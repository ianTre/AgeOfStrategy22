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
        public IEnumerator MoveToNewPosition(List<GridCell> path, Action onComplete);
        public void Action();
        public void Attack(ISoldier targetSoldier);
        public void ReceiveDamage(float damageAmmount);
    }
}
