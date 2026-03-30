using Assets.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BattlefieldManager : MonoBehaviour
{
    List<UnitModel> PlayerInitialUnits;
    List<UnitModel> EnemyInitialUnits;
    public List<Unit> PlayerUnits;
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
        GameObject newUnit = Instantiate(intendedUnitToCreate.prefab , gridController.gridArray[x, y].transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity);
        Unit unit = newUnit.GetComponent<Unit>();
        if(unit == null)
        {
            Debug.LogError("BattlefieldManager: The prefab assigned to the DeployCardController does not have a Unit component.");
            return false;
        }
        unit.InitUnit(intendedUnitToCreate, gridCell);
        gridCell.OcuppyNewUnit(unit);
        PlayerUnits.Add(unit);
        EnemyUnits.Add(unit);
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

    public Unit ReturnSelectedUnit()
    {
        var selectedUnits = PlayerUnits.FindAll(u => u.hasBeingSelected);
        if (selectedUnits != null && selectedUnits.Count > 0)
            return selectedUnits[0];
        return null;
    }

    public void MovementCellSelected(GridCell destiny)
    {
        Unit selectedUnit = PlayerUnits.Find(u => u.hasBeingSelected);
        if(selectedUnit == null)
            return;
        if(destiny.isOccupied)
            return;
        selectedUnit.MoveToNewCell(destiny);
    }

    public List<GridCell> FindShortestPath(GridCell origin, GridCell destiniy)
    {
        return gridController.FindShortestPath(origin, destiniy);
    }

    public void TestDirection(GridCell initial , GridCell destiny, int step , int direction)
    {
        
    }

    public void MoveToSelectedCell(GridCell destiny)
    {
        if(destiny.isOccupied)
            return;
        Unit selectedUnit = PlayerUnits.Find(u => u.hasBeingSelected);
        if(selectedUnit == null)
            return;
        selectedUnit.MoveToNewCell(destiny);
    }

    public void SpecialOrder1()
    {
        Unit selectedUnit = PlayerUnits.Find(u => u.hasBeingSelected);
        if (selectedUnit == null)
            return;
        selectedUnit.GetComponent<UnitAnimationController>().TriggerAttack();
    }

    public void SpecialOrder2()
    {
        Unit selectedUnit = PlayerUnits.Find(u => u.hasBeingSelected);
        if (selectedUnit == null)
            return;
        selectedUnit.GetComponent<UnitAnimationController>().TriggerDefend();
    }

    public void SpecialOrder3()
    {
        Unit selectedUnit = PlayerUnits.Find(u => u.hasBeingSelected);
        if (selectedUnit == null)
            return;
        selectedUnit.GetComponent<UnitAnimationController>().TriggerDeath();
    }

    public void SpecialOrder4()
    {
        throw new NotImplementedException();
    }

    public void SpecialOrder5()
    {
        throw new NotImplementedException();
    }

    public void SpecialOrder6()
    {
        throw new NotImplementedException();
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
