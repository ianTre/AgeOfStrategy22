using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
                unit.Id = repository.AddUnit(playerId, unit.data.unitType, unit.data.health * unit.number , unit.cell.x, unit.cell.y, unit.number , unit.data);
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

        public PlayerUnit FindPlayerUnitById(int id)
        {
            return repository.units.FirstOrDefault(u => u.id == id);
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
            PlayerUnit defenderUnit = FindPlayerUnitById(defender.Id);
            PlayerUnit attackerUnit = FindPlayerUnitById(attacker.Id);
            if( defenderUnit == null || attackerUnit == null)
                throw new Exception("Target or Attacker unit not found in the repository");

            /*int finalDamage = CalculateDamage(attackerUnit, defenderUnit);
            RecibirImpacto(defenderUnit, finalDamage);
            CalcularVida(defenderUnit);*/

            int baseDamage = attackerUnit.unitData.attack - defenderUnit.unitData.meleeArmor;
            int damage = Math.Max(1, baseDamage);
            RecibirImpacto(defenderUnit, damage);
            if(defenderUnit.healthTotal <= 0)
            {
                defender.IsAlive(false);
            }
            var healthBarController = defender.transform.GetComponentInChildren<HealthBarController>();
            healthBarController?.calculateDamage(defenderUnit.healthTotal);
            return;
        }

        /*public static int CalculateDamage(PlayerUnit attacker, PlayerUnit defender)
        {
            float scaledQuantity = (float)Math.Pow(attacker.ammount, 0.9f);
            float baseDamage = attacker.unitData.attack * scaledQuantity;

            float damageAfterArmor =
                baseDamage * (ARMOR_CONSTANT / (float)(ARMOR_CONSTANT + defender.unitData.meleeArmor));

            int finalDamage = (int)Math.Max(1, MathF.Round(damageAfterArmor));

            return finalDamage;
        }*/

        private static int CalculateDamage(PlayerUnit attacker, PlayerUnit defender)
        {
            float scaledQuantity = (float)Math.Pow(attacker.ammount, 0.9f);
            float baseDamage = attacker.unitData.attack * scaledQuantity;

            float damageAfterArmor =
                baseDamage * ((float)100 / (100 + defender.unitData.meleeArmor));

            int finalDamage = (int)Math.Max(1, MathF.Round(damageAfterArmor));

            return finalDamage;
        }

        private void RecibirImpacto(PlayerUnit unit, int damage)
        {
            int totalHealth = unit.healthTotal - damage;
            totalHealth = Math.Max(0, totalHealth);
            unit.healthTotal = totalHealth;
        }

        private void CalcularVida(PlayerUnit unit)
        {
            unit.ammount = (int)Math.Ceiling((float)unit.healthTotal / unit.unitData.Health);
        }

    }
}
