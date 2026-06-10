
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;


namespace Assets.Assets.Scripts.Managers
{
    public class AIEnemyManager
    {
        private GridController gridController;

        public AIEnemyManager(GridController gridController)
        {
            this.gridController = gridController;
        }

        public Unit ChooseNextUnit(int turn)
        {
            List<Unit> enemyUnits = BattlefieldManager.instance.EnemyUnits.Where(unit => unit.CoolDownTurn <= turn && unit.isAlive).ToList();
            Unit unitToMove = null;
            while (unitToMove == null && enemyUnits.Count > 0)
            {
                int randomUnit = UnityEngine.Random.Range(0, enemyUnits.Count);
                unitToMove = enemyUnits[randomUnit];
            }
            return unitToMove;
        }

        public void ChooseNextMovement(Unit unitToMove)
        {
            GridCell cellToMove = null;
            
            bool movementOk = false;
            while (!movementOk)
            {
                cellToMove = FindNextMovementOrAction(unitToMove);

                if (cellToMove == null)
                {
                    Debug.Log("Unit" + unitToMove.name + "couldnt find a possible movement");
                    GameManager.instance.StageUpdate_PlayerTurn();
                }


                movementOk = BattlefieldManager.instance.TryToMoveToNewCell(cellToMove, unitToMove, () =>
                {
                    Unit unit = FindPlayerTargetsToHit(unitToMove);
                    if (unit)
                    {
                        BattlefieldManager.instance.TryToAttackTarget(unitToMove, unit, () => { });
                    }
                    GameManager.instance.StageUpdate_PlayerTurn();
                });
            }
        }

        private Unit FindPlayerTargetsToHit(Unit unitToMove)
        {
            Unit target = null;
            var adyacentCells = gridController.findAllCellsAroundTarget(unitToMove.cell);
            var playerUnits = BattlefieldManager.instance.PlayerUnits;
            var targetCell = adyacentCells.Where(cell => cell.isOccupied && cell.unit != null && playerUnits.Contains(cell.unit)).FirstOrDefault();
            if (targetCell)
            {
                target = targetCell.unit;
            }
            return target;
        }

        private GridCell FindNextMovementOrAction(Unit unitToMove)
        {
            GridCell cellToMove = null;
            var possibleMovments = gridController.cellsList.Where(cell => cell.canBeActionable).ToList();
            if (possibleMovments.Count == 0)
                return null;

            var playerUnits = BattlefieldManager.instance.PlayerUnits.Where(u => u.isAlive);
            var possibleTargets = possibleMovments.Where(cell => cell.isOccupied && cell.unit != null).ToList(); //List all cells with Player units.
            possibleTargets = possibleTargets.Where(cell => playerUnits.Contains(cell.unit)).ToList();
            
            possibleMovments.Except(possibleTargets);
            
            bool hasAIFoundAction = false;

            if (possibleTargets.Count > 0)  //Exists Player unit whitin enemy range.
            {
                bool foundRightSpot = false;

                while (!foundRightSpot && possibleTargets.Count > 0)
                {
                    int targetIndex = Random.Range(0, possibleTargets.Count); //Player unit whitin enemy range.
                    var possibleTargetPosition = possibleTargets[targetIndex];

                    var cellsArroundTargetALL = gridController.FindFreeCellsAroundTarget(possibleTargetPosition);
                    var cellsWithinRange = cellsArroundTargetALL.Where(cell => cell.canBeActionable).ToList();
                    if (cellsWithinRange.Count == 0)
                    {
                        possibleTargets.Remove(possibleTargetPosition);
                    }
                    else
                    {
                        cellToMove = cellsWithinRange[0];
                        foundRightSpot = true;
                        hasAIFoundAction = true;
                    }
                }
            }

            if(!hasAIFoundAction && possibleMovments.Count > 0)
            {
                var NoAggresiveMovements = possibleMovments.Where(cell => !cell.isOccupied).ToList();
                NoAggresiveMovements = NoAggresiveMovements.Where(cell => cell.unit == null).ToList();

                if (NoAggresiveMovements.Count > 0)
                {

                    int targetIndex = Random.Range(0, playerUnits.Count()); //Player unit whitin enemy range.
                    Unit playerUnittoBeSelectedbyAI = playerUnits.ToList()[targetIndex]; 
                    cellToMove = NoAggresiveMovements.OrderBy(cell => Vector3.Distance(cell.transform.position , playerUnittoBeSelectedbyAI.transform.position)).FirstOrDefault();
                    //TODO CHECK THIS FUNCTION
                }
            }
            return cellToMove;
        }
    }
}
