using Assets.Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialUnitsSetup : MonoBehaviour
{
    [SerializeField] bool equalBattle;
    [SerializeField] int ManAtArmsAmmount;
    [SerializeField] int ManAtArmsAmmountEnemy;
    [SerializeField] UnitData ManAtArms;
    [SerializeField] int SpiermanAmmount;
    [SerializeField] int SpiermanAmmountEnemy;
    [SerializeField] UnitData Spierman;
    [SerializeField] int KnightAmmount;
    [SerializeField] int KnightAmmountEnemy;
    [SerializeField] UnitData Knight;
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
