using Assets.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    List<UnitModel> PlayerUnits;
    List<UnitModel> EnemyUnits;
    private GridController gridController;
    private UnitData intendedUnitToCreate;
    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        PlayerUnits = new List<UnitModel>();
        EnemyUnits = new List<UnitModel>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool TryAddUnit(GridCell cell)
    {
        int x = cell.x;
        int y = cell.y;
        if (gridController.gridArray[x, y].GetComponent<GridCell>().isOccupied)
            return false;
        gridController.gridArray[x, y].GetComponent<GridCell>().isOccupied = true;
        Instantiate(intendedUnitToCreate.prefab , gridController.gridArray[x, y].transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        //PlayerUnits.Add(intendedUnitToCreate);
        return true;
    }

    internal void SetUnitToCreate(UnitData unitData)
    {
        intendedUnitToCreate = unitData;
    }
}
