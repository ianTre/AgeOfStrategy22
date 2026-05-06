using Assets.Assets.Scripts;
using Assets.Assets.Scripts.Backend;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;
using static GameManager;
using static UnityEngine.UI.CanvasScaler;
using Random = UnityEngine.Random;

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
    private int intendedUnitAmountToCreate;
    private Unit targetEnemyUnit;
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
        newUnit.AddComponent<PlayerUnit>();
        unit.InitUnit(intendedUnitToCreate, gridCell, intendedUnitAmountToCreate);
        gridCell.OcuppyNewUnit(unit);
        PlayerUnits.Add(unit);
        lastClickedCard.DisableCard();
        return true;
    }

    private bool CreateEnemyUnit(int xPos ,int yPos , UnitData unitData , int amount)
    {
        GridCell gridCell = gridController.gridArray[xPos, yPos].GetComponent<GridCell>();
        if (!gridCell || !gridCell.TryOcuppieCell())
            return false;        
        GameObject newUnit = Instantiate(unitData.prefab, gridController.gridArray[xPos, yPos].transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity);
        Unit unit = newUnit.GetComponent<Unit>();
        if (unit == null)
        {
            Debug.LogError("BattlefieldManager: The prefab assigned to the DeployCardController does not have a Unit component.");
            return false;
        }
        unit.InitUnit(unitData, gridCell, amount);
        gridCell.OcuppyNewUnit(unit);
        EnemyUnits.Add(unit);
        return true;
    }

    private void CreateEnemyUnits(List<UnitModel> enemyUnits)
    {
        foreach (UnitModel unit in enemyUnits)
        {
            bool created = false;
            while(!created)
            {
                int y = Random.Range(0, 2);
                int x = Random.Range(0, gridController.columns);
                created = CreateEnemyUnit(x, y, unit.InitialData, unit.number);
            }
            
        }

    }

    internal void SetUnitToCreate(DeployCardController deployCardController)
    {
        lastClickedCard = deployCardController;
        intendedUnitToCreate = deployCardController.unitsInitialData;
        intendedUnitAmountToCreate = deployCardController.unitsLeftToBeCreated;
    }

    internal void SetPlayerInitialUnits(List<UnitModel> playerUnits)
    {
        this.PlayerInitialUnits = playerUnits;
    }

    public void SelectANewUnit(Unit unit)
    {
        var selectedUnits = PlayerUnits.FindAll(u => u.transform.GetComponent<PlayerUnit>().hasBeingSelected && u != unit);
        if (selectedUnits != null && selectedUnits.Count > 0)
            selectedUnits.ForEach(u => u.transform.GetComponent<PlayerUnit>().Deselect());
    }

    public Unit GetSelectedUnit()
    {
        var selectedUnit = PlayerUnits.FirstOrDefault(u => u.transform.GetComponent<PlayerUnit>().hasBeingSelected);
        return selectedUnit;
    }

    public Unit GetTargetUnit()
    {
        var selectedUnit = PlayerUnits.FirstOrDefault(u => u.transform.GetComponent<PlayerUnit>().hasBeingSelected);
        return selectedUnit;
    }

    public void DeselectUnits()
    {
        var selectedUnits = PlayerUnits.FindAll(u => u.transform.GetComponent<PlayerUnit>().hasBeingSelected);
        if (selectedUnits != null && selectedUnits.Count > 0)
            selectedUnits.ForEach(u => u.transform.GetComponent<PlayerUnit>().Deselect());
    }

    public Unit ReturnSelectedUnit()
    {
        var selectedUnits = PlayerUnits.FindAll(u => u.transform.GetComponent<PlayerUnit>());
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


    public List<GridCell> FindShortestPath(GridCell origin, GridCell destiniy)
    {
        return gridController.FindShortestPath(origin, destiniy);
    }

    public void TestDirection(GridCell initial , GridCell destiny, int step , int direction)
    {
        
    }

    public bool TryToMoveToNewCell(GridCell newCell, Unit unitToMove , Action onComplete)
    {
        if (newCell.isOccupied)
            return false;
        unitToMove.path = FindShortestPath(unitToMove.cell, newCell);
        if (unitToMove.path != null && unitToMove.path.Count > 0)
        {
            backendService.MoveUnitOnMap(newCell, unitToMove);
            unitToMove.cell.RemoveUnit(unitToMove); //Clean Old Cell
            unitToMove.cell = newCell;
            unitToMove.cell.OcuppyNewUnit(unitToMove); //Assign new Cell
            StartCoroutine(unitToMove.MoveToNewPosition(unitToMove.path , onComplete));
            
            return true;
        }
        return false;
    }


    public bool TryToAttackTarget(Unit attacker, Unit target, Action onComplete)
    {
        if (target == null || attacker == null)
            return false;
        if (!IsEnemyNearBy(attacker.cell))
            return false;
        backendService.AttackUnit(target, attacker);
        StartCoroutine(attacker.AttackTarget(target, onComplete));
        return true;
    }

    public void StartBattleAction(List<UnitModel> enemyUnits)
    {
        if (!allUnitsDeployed)
            return;
        backendService.DeployPlayerUnits(1, PlayerUnits);
        CreateEnemyUnits(enemyUnits);
        backendService.DeployPlayerUnits(2 ,EnemyUnits);
    }


    public void SpecialOrder1()
    {
        Unit selectedUnit = PlayerUnits.Find(u => u.transform.GetComponent<PlayerUnit>().hasBeingSelected);
        if (selectedUnit == null)
            return;
        selectedUnit.GetComponent<UnitAnimationController>().TriggerAttack();
    }

    public void SpecialOrder2()
    {
        Unit selectedUnit = PlayerUnits.Find(u => u.transform.GetComponent<PlayerUnit>().hasBeingSelected);
        if (selectedUnit == null)
            return;
        selectedUnit.GetComponent<UnitAnimationController>().TriggerDefend();
    }

    public void SpecialOrder3()
    {
        Unit selectedUnit = PlayerUnits.Find(u => u.transform.GetComponent<PlayerUnit>().hasBeingSelected);
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
        
    }

    internal void SelectTargetUnit(Unit target)
    {
        Unit playerUnit = GetSelectedUnit();
        if (playerUnit == null)
        {
            Debug.Log("Player unit selected not Found in SelectTargetUnit");
            GameManager.instance.StageUpdate_PlayerTurn();
        }

        if (!GameManager.instance.CheckStage(GameManager.GameStages.PlayerTurn_UnitSelected))
            return;
        targetEnemyUnit = target;
        var cells = gridController.FindAllNeighbours(target.cell);
        foreach (var cell in cells) 
            cell.Highlight(true);
        GameManager.instance.StageUpdate_PlayerTurn_UnitSelected_EnemyTargetSelected(playerUnit, target);
    }

    internal Unit GetTargetEnemyUnit()
    {
        return targetEnemyUnit;
    }

    internal void SaveAttack(Unit playerUnit, Unit targetUnit)
    {
        //TODO : Save Attack to send to backend when the player confirms the attack action
        return;
    }
}
