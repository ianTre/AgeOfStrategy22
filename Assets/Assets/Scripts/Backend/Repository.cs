using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Assets.Scripts.Backend
{
    public class Repository
    {
        public List<PlayerUnit> units = new List<PlayerUnit>();
        int idIndexer = 0;

        public void AddUnit(int playerId ,UnitType unitType, int health , int xPos , int yPos , int ammount , UnitData unitData)
        {
            if(units.Exists(u => u.xPos == xPos && u.yPos == yPos))
            {
                throw new Exception("There is already an unit in this position");
            }
            PlayerUnit unit = new PlayerUnit(playerId , unitType , health , xPos , yPos, ammount, ++idIndexer , unitData);
            units.Add(unit);
        }

        internal PlayerUnit GetPlayerUnit(GridCell cell, UnitType unitType, int health, int playerId)
        {
            var playerUnits = units.Where(u => u.playerId == playerId).ToList();
            return this.units.FirstOrDefault(unit => unit.xPos == cell.x && unit.yPos == cell.y && unit.unitType == unitType && unit.healthTotal == health && unit.playerId == playerId);
        }

        internal List<PlayerUnit> GetPlayerUnits(int playerId)
        {
            return units.Where(u => u.playerId == playerId).ToList();
        }

        internal void MoveUnit(int unitId, int x, int y)
        {
            PlayerUnit unit = units.FirstOrDefault(u => u.id == unitId);
            if(unit == null)
            {
                throw new Exception("Unit not found");
            }
            unit.xPos = x;
            unit.yPos = y;
        }
    }

    public class PlayerUnit
    {
        public int playerId;
        public UnitType unitType;
        public int healthTotal;
        public int ammount;
        public int xPos;
        public int yPos;
        public int id;
        public UnitData unitData;

        public PlayerUnit(int playerId , UnitType unitType , int health , int xPos , int yPos ,int ammount , int id , UnitData unitData)
        {
            this.playerId = playerId;
            this.unitType = unitType;
            this.healthTotal = health;
            this.ammount = ammount;
            this.xPos = xPos;
            this.yPos = yPos;
            this.id = id;
            this.unitData = unitData;
        }
    }
}
