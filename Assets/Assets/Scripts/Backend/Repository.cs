using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Assets.Scripts.Backend
{
    public class Repository
    {
        public List<PlayerUnit> units = new List<PlayerUnit>();

        public void AddUnit(int playerId ,UnitType unitType, int health , int xPos , int yPos)
        {
            if(units.Exists(u => u.xPos == xPos && u.yPos == yPos))
            {
                throw new Exception("There is already an unit in this position");
            }
            PlayerUnit unit = new PlayerUnit(playerId , unitType , health , xPos , yPos);
            units.Add(unit);
        }

        internal List<PlayerUnit> GetPlayerUnits(int playerId)
        {
            return units.Where(u => u.playerId == playerId).ToList();
        }
    }

    public class PlayerUnit
    {
        public int playerId;
        public UnitType unitType;
        public int health;
        public int xPos;
        public int yPos;

        public PlayerUnit(int playerId , UnitType unitType , int health , int xPos , int yPos)
        {
            this.playerId = playerId;
            this.unitType = unitType;
            this.health = health;
            this.xPos = xPos;
            this.yPos = yPos;
        }
    }
}
