using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Assets.Assets.Scripts.Backend
{
    internal class Service
    {
        Repository repository;
        const int ARMOR_CONSTANT = 100;

        public Service() 
        {
            repository = new Repository();
        }

        public void DeployPlayerUnits(int playerId , List<Unit> playerUnits)
        {
            foreach (var unit in playerUnits)
            {
                repository.AddUnit(playerId, unit.data.unitType, unit.data.health * unit.number , unit.cell.x, unit.cell.y, unit.number , unit.data);
            }
        }

        public List<PlayerUnit> GetPlayerUnits(int playerId)
        {
            return repository.GetPlayerUnits(playerId);
        }

        public PlayerUnit FindUnit(Unit unit)
        {
            int PlayerId = IsPlayerIdByStage() ? 1 : 2;
            return repository.GetPlayerUnit(unit.cell , unit.data.unitType , unit.data.health * unit.number , PlayerId);
        }

        private bool IsPlayerIdByStage()
        {
            List<GameManager.GameStages> playerStages = new List<GameManager.GameStages>()
            {
                GameManager.GameStages.PlayerTurn,
                GameManager.GameStages.PlayerTurn_UnitSelected,
                GameManager.GameStages.PlayerTurn_UnitSelected_EnemyTargetSelected,
                GameManager.GameStages.PlayerTurn_UnitSelected_EnemyTargetSelected_PositionToAttackSelected,
                GameManager.GameStages.PlayerTurn_UnitSelected_EnemyTargetSelected_PositionToAttackSelected_attackMade
            };
            if (GameManager.instance.CheckStages(playerStages))
                return true;
            return false;
        }

        public PlayerUnit FindEnemyUnit(Unit unit)
        {
            int PlayerId = IsPlayerIdByStage() ? 2 : 1;
            return repository.GetPlayerUnit(unit.cell, unit.data.unitType, unit.data.health * unit.number, PlayerId);
        }

        internal void MoveUnitOnMap(GridCell gridCell, Unit unitToMove)
        {
            PlayerUnit unit = FindUnit(unitToMove);
            if (unit != null)
            {
                repository.MoveUnit(unit.id, gridCell.x, gridCell.y);
            }
        }

        internal void AttackUnit(Unit defender, Unit attacker)
        {
            PlayerUnit defenderUnit = FindEnemyUnit(defender);
            PlayerUnit attackerUnit = FindUnit(attacker);
            if( defenderUnit == null || attackerUnit == null)
                throw new Exception("Target or Attacker unit not found in the repository");

            // 2. Calcular daño base con eficiencia decreciente
            float scaledQuantity = Mathf.Pow(attacker.number, 0.9f);
            float baseDamage = attacker.data.attack * scaledQuantity;

            // 4. Reducción por armadura
            float damageAfterArmor = baseDamage *(ARMOR_CONSTANT / (ARMOR_CONSTANT + defender.data.meleeArmor));
            int finalDamage = Mathf.RoundToInt(damageAfterArmor);

            defenderUnit.healthTotal -= finalDamage;
            defenderUnit.ammount = Mathf.CeilToInt((float)defenderUnit.healthTotal / defender.data.health);

            return;
        }
    }
}
