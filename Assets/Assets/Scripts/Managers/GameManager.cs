using Assets.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameStages gameStage;
    private GameStages oldGameStage;
    private InitialUnitsSetup initialUnitsSetup;
    private UIManager uIManager;
    private GameObject lastSelectedObject;
    public static GameManager instance;

    //Nedded References
    BattlefieldManager battlefieldManager;


    void Start()
    {
        battlefieldManager = GetComponent<BattlefieldManager>();
        uIManager = GetComponent<UIManager>();
        StageUpdate_PlayerDeploy();
    }

    private void Awake()
    {
        instance = this;
        gameStage = GameStages.Start;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStage != oldGameStage)
        {
            switch (gameStage)
            {
                case GameStages.Start:
                    break;
                case GameStages.PlayerDeploy:
                    break;
                case GameStages.PlayerDeploy_CardSelected:
                    break;
                case GameStages.PlayerDeploy_CardSelected_CellSelected:
                    break;
                case GameStages.EnemyDeploy:
                    break;
                case GameStages.PlayerTurn:
                    InputManager.instance.EnableControllers();
                    break;
                case GameStages.PlayerTurn_UnitSelected:
                    break;
                case GameStages.PlayerTurn_UnitSelected_UnitMovement:
                    InputManager.instance.DisableControllers();
                    break;
                case GameStages.PlayerTurn_UnitSelected_EnemyTargetSelected:
                    break;
                case GameStages.EnemyTurn:
                    break;
                default:
                    break;
            }
            oldGameStage = gameStage;
        }
    }


    public void ClickOnNewObject(GameObject newSelectedObject)
    {
        IMouseActionable lastActionable = null;
        lastSelectedObject?.TryGetComponent<IMouseActionable>(out lastActionable);
        newSelectedObject = FindActionableRecursibly(newSelectedObject);
        newSelectedObject.TryGetComponent<IMouseActionable>(out IMouseActionable actionable);

        if (gameStage == GameStages.PlayerDeploy)
        {
            if (lastActionable != null)
                lastActionable.Deselect();
            if (actionable != null)
                actionable.Select();
        }

        if (gameStage == GameStages.PlayerDeploy_CardSelected)
        {
            if (newSelectedObject.tag == "Grid")
            {
                newSelectedObject.TryGetComponent<GridCell>(out GridCell gridCell);
                StageUpdate_PlayerDeploy_CardSelected_CellSelected(gridCell);
            }
        }

        if (gameStage == GameStages.PlayerTurn || gameStage == GameStages.PlayerTurn_UnitSelected)
        {
            if (lastActionable != null)
                lastActionable.Deselect();
            if (actionable != null)
                actionable.Select();

            Unit unit = battlefieldManager.PlayerUnits.Find(u => u.transform.GetComponent<PlayerUnit>().hasBeingSelected);
            if (unit != null)
            {
                gameStage = GameStages.PlayerTurn_UnitSelected;
            }
            else
            {
                gameStage = GameStages.PlayerTurn;
            }
        }

        lastSelectedObject = newSelectedObject;
    }

    public void RightClickOnNewObject(GameObject newSelectedObject)
    {
        newSelectedObject = FindActionableRecursibly(newSelectedObject);
        newSelectedObject.TryGetComponent<IMouseActionable>(out IMouseActionable actionable);

        if (gameStage == GameStages.PlayerTurn_UnitSelected)
        {
            if (actionable != null)
                actionable.Action();
        }
    }

    private GameObject FindActionableRecursibly(GameObject newSelectedObject)
    {
        var parent = newSelectedObject;
        while (parent != null)
        {
            parent.TryGetComponent<IMouseActionable>(out IMouseActionable actionable);
            if (actionable != null)
            {
                return parent.gameObject;
            }
            parent = parent.transform.parent != null ? parent.transform.parent.gameObject : null;
        }
        return newSelectedObject;
    }

    public void CellCalledForAction(GridCell cell)
    {
        var selectedUnit = battlefieldManager.GetSelectedUnit();
        if (selectedUnit != null)
            StageUpdate_PlayerTurn_UnitSelected_UnitMovement(selectedUnit, cell);
    }

    public void SpecialAction()
    {
        gameStage = GameStages.PlayerTurn;
    }

    public void SpecialOrder1()
    {
        battlefieldManager.SpecialOrder1();
    }

    public void SpecialOrder2()
    {
        battlefieldManager.SpecialOrder2();
    }

    public void SpecialOrder3()
    {
        battlefieldManager.SpecialOrder3();
    }

    public void SpecialOrder4()
    {
        battlefieldManager.SpecialOrder4();
    }

    public void SpecialOrder5()
    {
        battlefieldManager.SpecialOrder5();
    }

    public void SpecialOrder6()
    {
        battlefieldManager.SpecialOrder6();
    }

    private void SetInitialDataAndDeploy(List<UnitModel> PlayerUnits, List<UnitModel> EnemyUnits)
    {
        //Prepare and Show UI
        uIManager.Battlefield_UnitsDeploy_Canvas(PlayerUnits);
        battlefieldManager.SetPlayerInitialUnits(PlayerUnits);
    }

    public void EndDeployButton()
    {
        List<UnitModel> EnemyUnits = initialUnitsSetup.EnemyInitialSetup();
        StageUpdate_EnemyDeploy(EnemyUnits);
    }

    public void StageUpdate_PlayerDeploy()
    {
        initialUnitsSetup = GetComponent<InitialUnitsSetup>();
        List<UnitModel> PlayerUnits = initialUnitsSetup.PlayerInitialSetup();
        List<UnitModel> EnemyUnits = initialUnitsSetup.EnemyInitialSetup();

        //Actual call for Stage Update
        gameStage = GameStages.PlayerDeploy;
        SetInitialDataAndDeploy(PlayerUnits, EnemyUnits);
    }

    public void StageUpdate_PlayerDeploy_CardSelected(DeployCardController deployCardController)
    {
        gameStage = GameStages.PlayerDeploy_CardSelected;
        battlefieldManager.SetUnitToCreate(deployCardController);
    }

    public void StageUpdate_PlayerDeploy_CardSelected_CellSelected(GridCell selectedCell)
    {
        if (selectedCell != null)
        {
            gameStage = GameStages.PlayerDeploy_CardSelected_CellSelected;
            var isSuccess = battlefieldManager.TryAddUnit(selectedCell);
            if (isSuccess)
            {
                gameStage = GameStages.PlayerDeploy;
            }
        }
    }

    public void StageUpdate_EnemyDeploy(List<UnitModel> enemyUnits)
    {
        gameStage = GameStages.EnemyDeploy;
        InputManager.instance.DisableControllers();
        battlefieldManager.StartBattleAction(enemyUnits);
        var deployCanvas = GameObject.Find("Battlefield_UnitsDeploy_Canvas");
        if (deployCanvas == null)
            throw new Exception("Cant find Battlefield_UnitsDeploy_Canvas");
        deployCanvas.SetActive(false);
        StageUpdate_PlayerTurn();
    }

    public void StageUpdate_PlayerTurn()
    {
        gameStage = GameStages.PlayerTurn;
        InputManager.instance.EnableControllers();
    }

    public void StageUpdate_PlayerTurn_UnitSelected(Unit selectedUnit)
    {
        BattlefieldManager.instance.SelectANewUnit(selectedUnit);
        gameStage = GameStages.PlayerTurn_UnitSelected;
    }


    public void StageUpdate_PlayerTurn_UnitSelected_UnitMovement(Unit selectedUnit, GridCell destinyCell)
    {
        gameStage = GameStages.PlayerTurn_UnitSelected_UnitMovement;
        InputManager.instance.DisableControllers();
        battlefieldManager.MoveToSelectedCell(selectedUnit, destinyCell);
    }

    public void StageUpdate_EnemyTurn()
    {
        gameStage = GameStages.EnemyTurn;
        StageUpdate_PlayerTurn();
    }






    public enum GameStages
    {
        Start = 0,
        PlayerDeploy = 10,
        PlayerDeploy_CardSelected = 11,
        PlayerDeploy_CardSelected_CellSelected = 12,
        EnemyDeploy = 20,
        PlayerTurn = 100,
        PlayerTurn_UnitSelected = 110,
        PlayerTurn_UnitSelected_UnitMovement = 111,
        PlayerTurn_UnitSelected_EnemyTargetSelected = 112,
        EnemyTurn = 200,
    }
}
