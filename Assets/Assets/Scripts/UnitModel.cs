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
        public UnitData InitialData { get; set; }
        public int number;


        public UnitModel(UnitData initialData , int initialNumber)
        {
            this.InitialData = initialData;
            this.number = initialNumber;
        }
    }

    public enum UnitType
    {
        Alabardier,
        Axeman,
        GriffinCrusader,
        Knight,
        MaceWarrior,
        ManAtArms,
        Spierman,
        WarAxeSoldier
    }
}
