using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Assets.Scripts.Interfaces
{
    public interface ISoldier
    {
        public void MoveToNewCell(GridCell newCell);
        public IEnumerator MoveToNewPosition(List<GridCell> path);
        public void Action();
        public void Attack(ISoldier targetSoldier);
        public void ReceiveDamage(float damageAmmount);
    }
}
