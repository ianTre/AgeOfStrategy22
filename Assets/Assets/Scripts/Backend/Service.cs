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

    }
}
