using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Assets.Scripts.Managers
{
    public class AIEnemyManager
    {
        private GridController gridController;

        public AIEnemyManager(GridController gridController)
        {
            this.gridController = gridController;
        }

        public void ChooseNextMovement()
        {
            List<Unit> enemyUnits = BattlefieldManager.instance.EnemyUnits;
            Unit unitToMove = null;
            while (unitToMove == null && enemyUnits.Count > 0)
            {
                int randomUnit = UnityEngine.Random.Range(0, enemyUnits.Count);
                unitToMove = enemyUnits[randomUnit];
            }
            GridCell cellToMove = null;
            while (cellToMove == null)
            {
                int randomX = UnityEngine.Random.Range(0, gridController.columns);
                int randomY = UnityEngine.Random.Range(0, gridController.rows);
                GridCell potentialCell = gridController.gridArray[randomX, randomY]?.GetComponent<GridCell>();
                if (potentialCell != null && !potentialCell.isOccupied)
                {
                    cellToMove = potentialCell;
                }
            }
            bool movementOk = false;
            while (!movementOk)
            {
                movementOk = BattlefieldManager.instance.TryToMoveToNewCell(cellToMove,unitToMove, () =>
                {
                    GameManager.instance.StageUpdate_PlayerTurn();
                });
            }
        }
    }
}
