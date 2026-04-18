using Assets.Assets.Scripts;
using Assets.Assets.Scripts.Backend;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BattlefieldManager : MonoBehaviour
{
    private DeployCardController lastClickedCard;
    public static BattlefieldManager instance;
    private GridController gridController;
    private Service backendService;

    List<UnitModel> PlayerInitialUnits;
    List<UnitModel> EnemyInitialUnits;
    public List<Unit> PlayerUnits;
    public List<Unit> EnemyUnits;
    
    private UnitData intendedUnitToCreate;
    public GameObject selectionLightPrefab;
    
    public bool allUnitsDeployed = true; //TODO
    void Start()
    {
        gridController = FindObjectOfType<GridController>();
    }

    public void Awake()
    {
        instance = this;
        PlayerInitialUnits = new List<UnitModel>();
        EnemyInitialUnits = new List<UnitModel>();
        EnemyUnits = new List<Unit>();
        PlayerUnits = new List<Unit>();
        backendService = new Service();
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
        lastClickedCard.DisableCard();
        return true;
    }

    private void CreateEnemyUnits()
    {
        //Add Unit1
        int enemyXPos = 2;
        int enemyYPos = 1;
        GridCell gridCell = gridController.gridArray[enemyXPos, enemyYPos].GetComponent<GridCell>();
        if (!gridCell || !gridCell.TryOcuppieCell())
            return;        
        GameObject newUnit = Instantiate(EnemyInitialUnits[0].InitialData.prefab, gridController.gridArray[enemyXPos, enemyYPos].transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity);
        Unit unit = newUnit.GetComponent<Unit>();
        if (unit == null)
        {
            Debug.LogError("BattlefieldManager: The prefab assigned to the DeployCardController does not have a Unit component.");
            return;
        }
        unit.InitUnit(EnemyInitialUnits[0].InitialData, gridCell);
        gridCell.OcuppyNewUnit(unit);
        EnemyUnits.Add(unit);
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

    public bool IsEnemyNearBy(GridCell gridCell)
    {
        if (EnemyUnits == null || EnemyUnits.Count == 0) return false;

        var anyNear = EnemyUnits.Exists(u => u.cell.IsGridCellNearToMe(gridCell));
        return anyNear;
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

    public void StartBattleAction()
    {
        if (!allUnitsDeployed)
            return;
        backendService.DeployPlayerUnits(1, PlayerUnits);
        CreateEnemyUnits();
        backendService.DeployPlayerUnits(2 ,PlayerUnits);
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
