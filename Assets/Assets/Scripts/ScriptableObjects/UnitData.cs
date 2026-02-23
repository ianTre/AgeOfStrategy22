using Assets.Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Units/Unit Data")]
public class UnitData : ScriptableObject
{
    [SerializeField] private string unitName;
    //[SerializeField] private UnitType type;
    [SerializeField] private int health;
    [SerializeField] private int attack;
    [SerializeField] private int meleeArmor;
    [SerializeField] private int pierceArmor;
    [SerializeField] public GameObject prefab;

    public string UnitName { get { return name; } }
    //public UnitType Type { get { return type; } }
    public int Health { get { return health; } set { health = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public int MeleeArmor { get { return meleeArmor; } set { meleeArmor = value; } }
    public int PierceArmor { get {return pierceArmor; } set { pierceArmor = value; } }

}
