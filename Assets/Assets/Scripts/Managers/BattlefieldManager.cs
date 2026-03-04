using Assets.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    List<UnitModel> PlayerUnits;
    List<UnitModel> EnemyUnits;
    private GridController gridController;
    private UnitData intendedUnitToCreate;
    private DeployCardController lastClickedCard;
    public GameObject selectionLightPrefab;
    public static BattlefieldManager instance;
    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        PlayerUnits = new List<UnitModel>();
        EnemyUnits = new List<UnitModel>();
    }

    public void Awake()
    {
        instance = this;
    }



    // Update is called once per frame
    void Update()
    {

    }

    public bool TryAddUnit(GridCell cell)
    {
        int x = cell.x;
        int y = cell.y;
        GridCell gridCell = gridController.gridArray[x, y].GetComponent<GridCell>();
        if (!gridCell || !gridCell.TryAddUnit(intendedUnitToCreate.prefab))
            return false;
        GameObject newUnit = Instantiate(intendedUnitToCreate.prefab , gridController.gridArray[x, y].transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Unit unit = newUnit.GetComponent<Unit>();
        unit.InitUnit(intendedUnitToCreate, gridCell);
        //PlayerUnits.Add(intendedUnitToCreate);
        lastClickedCard.DisableCard();
        return true;
    }

    internal void SetUnitToCreate(DeployCardController deployCardController)
    {
        lastClickedCard = deployCardController;
        intendedUnitToCreate = deployCardController.unitsInitialData;
    }


}
