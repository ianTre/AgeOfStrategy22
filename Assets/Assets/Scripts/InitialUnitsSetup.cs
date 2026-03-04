using Assets.Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialUnitsSetup : MonoBehaviour
{
    /// <summary>
    /// If true, the player and the enemy will have the same units in the same amount, and it will ignore the enemy ammoutn field. If false, they will be different.
    /// </summary>
    [SerializeField] bool equalBattle;
    [SerializeField] List<UnitInitialSetup> UnitList;

    private List<UnitModel> unitDataList;

    public InitialUnitsSetup()
    {
        unitDataList = new List<UnitModel>();
    }

    public List<UnitModel> PlayerInitialSetup()
    {
        foreach (UnitInitialSetup model in UnitList)
        {
            unitDataList.Add(new UnitModel(model.unitData, model.playerAmmount));
        }
        return unitDataList;
    }

    public List<UnitModel> EnemyInitialSetup()
    {
        if(equalBattle)
            return unitDataList;

        var list = new List<UnitModel>();
        foreach (UnitInitialSetup model in UnitList)
        {
            list.Add(new UnitModel(model.unitData, model.playerAmmount));
        }

        return list;
    }
}
