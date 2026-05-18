using Assets.Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Units/Unit Data")]
public class UnitData : ScriptableObject
{
    [SerializeField] public string unitName;
    //[SerializeField] private UnitType type;
    [SerializeField] public int health;
    [SerializeField] public int attack;
    [SerializeField] public int meleeArmor;
    [SerializeField] public int pierceArmor;
    [SerializeField] public int scope = 2;
    [SerializeField] public UnitType unitType;
    [SerializeField] public GameObject prefab;
    [SerializeField] public Sprite photo;

    public string UnitName { get { return name; } }
    //public UnitType Type { get { return type; } }
    public int Health { get { return health; } set { health = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public int MeleeArmor { get { return meleeArmor; } set { meleeArmor = value; } }
    public int PierceArmor { get {return pierceArmor; } set { pierceArmor = value; } }

}
