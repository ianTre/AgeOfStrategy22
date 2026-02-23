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

    private List<(UnitData, int)> unitDataList;

    public InitialUnitsSetup()
    {
        unitDataList = new List<(UnitData, int)>();
    }

    public List<(UnitData,int)> PlayerInitialSetup()
    {
        //Man At Arms
        if(ManAtArms != null && ManAtArmsAmmount > 0)
            unitDataList.Add((ManAtArms, ManAtArmsAmmount));

        //Spierman
        if(Spierman != null && SpiermanAmmount > 0)
            unitDataList.Add((Spierman, SpiermanAmmount));

        //Add new units here

        return unitDataList;
    }

    public List<(UnitData, int)> EnemyInitialSetup()
    {
        if(equalBattle)
            return unitDataList;


        var list = new List<(UnitData, int)>();

        //Man At Arms
        if (ManAtArms != null && ManAtArmsAmmountEnemy > 0)
            list.Add((ManAtArms, ManAtArmsAmmountEnemy));

        //Spierman
        if (Spierman != null && SpiermanAmmountEnemy > 0)
            list.Add((Spierman, SpiermanAmmountEnemy));

        //Add new units here

        return list;
    }
}
