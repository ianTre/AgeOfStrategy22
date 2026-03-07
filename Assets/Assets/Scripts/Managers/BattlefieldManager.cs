using Assets.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    List<UnitModel> PlayerInitialUnits;
    List<UnitModel> EnemyInitialUnits;
    List<Unit> PlayerUnits;
    public List<Unit> EnemyUnits;
    private GridController gridController;
    private UnitData intendedUnitToCreate;
    private DeployCardController lastClickedCard;
    public GameObject selectionLightPrefab;
    public static BattlefieldManager instance;
    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        PlayerInitialUnits = new List<UnitModel>();
        EnemyInitialUnits = new List<UnitModel>();
        EnemyUnits = new List<Unit>();
        PlayerUnits = new List<Unit>();
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
        if (!gridCell || !gridCell.TryOcuppieCell())
            return false;
        GameObject newUnit = Instantiate(intendedUnitToCreate.prefab , gridController.gridArray[x, y].transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Unit unit = newUnit.GetComponent<Unit>();
        unit.InitUnit(intendedUnitToCreate, gridCell);
        gridCell.OcuppyNewUnit(unit);
        PlayerUnits.Add(unit);
        lastClickedCard.DisableCard();
        return true;
    }

    internal void SetUnitToCreate(DeployCardController deployCardController)
    {
        lastClickedCard = deployCardController;
        intendedUnitToCreate = deployCardController.unitsInitialData;
    }

    internal void SetPlayerInitialUnits(List<UnitModel> playerUnits)
    {
        this.PlayerInitialUnits = playerUnits;
    }

    public void SetEnemyInitialUnits(List<UnitModel> enemyUnits)
    {
        this.EnemyInitialUnits = enemyUnits;
    }

    public void SelectANewUnit(Unit unit)
    {
        var selectedUnits = PlayerUnits.FindAll(u => u.hasBeingSelected && u != unit);
        if (selectedUnits != null && selectedUnits.Count > 0)
            selectedUnits.ForEach(u => u.Deselect());
    }

    public void DeselectUnits()
    {
        var selectedUnits = PlayerUnits.FindAll(u => u.hasBeingSelected);
        if (selectedUnits != null && selectedUnits.Count > 0)
            selectedUnits.ForEach(u => u.Deselect());
    }

    public bool TryMoveUnit(Unit unit , GridCell destiny)
    {
        if (destiny.isOccupied)
            return false;

        //TO BE CHANGED
        return true;
    }

    internal List<GridCell> FindShortestPath(GridCell cell, GridCell newCell)
    {
        var list = new List<GridCell>();
        //TO BE CHANGED
        GameObject gameObject1 = gridController.gridArray[cell.x, cell.y + 1];
        GameObject gameObject2 = gridController.gridArray[cell.x, cell.y + 2];
        GridCell gridCell1 = gameObject1.GetComponent<GridCell>();
        GridCell gridCell2 = gameObject2.GetComponent<GridCell>();
        list.Add(gridCell1);
        list.Add(gridCell2);
        //HARDCODED
        return list;
    }


    public void SpecialAction()
    {
        Unit selectedUnit = PlayerUnits.Find(u => u.hasBeingSelected);
        if (selectedUnit == null)
            return;
        var cell = gridController.gridArray[selectedUnit.cell.x, selectedUnit.cell.y + 2].GetComponent<GridCell>();
        if (cell == null)
                return;
        selectedUnit.MoveToNewCell(cell);
    }
}
