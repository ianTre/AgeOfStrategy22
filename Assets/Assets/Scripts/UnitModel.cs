using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Assets.Scripts
{
    public class UnitModel
    {
        private UnitData InitialData { get; set; }
        private int number;
        private int totalHealth;
        private int totalAttack;


        public UnitModel(UnitData initialData , int initialNumber)
        {
            this.InitialData = initialData;
            this.number = initialNumber;
            this.totalAttack = initialData.Health * initialNumber;
            this.totalAttack = initialData.Attack * initialNumber;
        }
    }

    public enum UnitType
    {
        SwordMan = 1,
        Champion = 10,
        Spearman = 2,
        Pikeman = 20,
        Archer = 3,
        Ranger = 30,
        Monk = 4,
    }
}
