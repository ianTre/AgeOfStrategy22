using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Assets.Scripts.Backend
{
    internal class Service
    {
        Repository repository;

        public Service() 
        {
            repository = new Repository();
        }

        public void DeployPlayerUnits(int playerId , List<Unit> playerUnits)
        {
            foreach (var unit in playerUnits)
            {
                repository.AddUnit(playerId, unit.data.unitType, unit.data.health, unit.cell.x, unit.cell.y);
            }
        }

        public List<PlayerUnit> GetPlayerUnits(int playerId)
        {
            return repository.GetPlayerUnits(playerId);
        }

        public PlayerUnit FindUnit(Unit unit)
        {
            int PlayerId = GameManager.instance.gameStage == GameManager.GameStages.EnemyTurn ? 2 : 1;
            return repository.GetPlayerUnit(unit.cell , unit.data.unitType , unit.data.health , PlayerId);
        }

        internal void MoveUnitOnMap(GridCell gridCell, Unit unitToMove)
        {
            PlayerUnit unit = FindUnit(unitToMove);
            if (unit != null)
            {
                repository.MoveUnit(unit.id, gridCell.x, gridCell.y);
            }
        }

        internal void AttackUnit(Unit target, Unit attacker)
        {
            return;
        }
    }
}
