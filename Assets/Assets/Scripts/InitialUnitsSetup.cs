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

    private List<UnitModel> unitDataList;

    public InitialUnitsSetup()
    {
        unitDataList = new List<UnitModel>();
    }

    public List<UnitModel> PlayerInitialSetup()
    {
        //Man At Arms
        if (ManAtArms != null && ManAtArmsAmmount > 0)
            unitDataList.Add(new UnitModel( ManAtArms, ManAtArmsAmmount));

        //Spierman
        if(Spierman != null && SpiermanAmmount > 0)
            unitDataList.Add(new UnitModel(Spierman, SpiermanAmmount));

        //Add new units here

        return unitDataList;
    }

    public List<UnitModel> EnemyInitialSetup()
    {
        if(equalBattle)
            return unitDataList;


        var list = new List<UnitModel>();

        //Man At Arms
        if (ManAtArms != null && ManAtArmsAmmountEnemy > 0)
            list.Add(new UnitModel(ManAtArms, ManAtArmsAmmountEnemy));

        //Spierman
        if (Spierman != null && SpiermanAmmountEnemy > 0)
            list.Add(new UnitModel(Spierman, SpiermanAmmountEnemy));

        //Add new units here

        return list;
    }
}
