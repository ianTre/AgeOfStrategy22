using Assets.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameStages gameStage;
    private InitialUnitsSetup initialUnitsSetup;
    private UIManager uIManager;
    private GameObject lastSelectedObject;


    //Nedded References
    BattlefieldManager battlefieldManager;


    void Start()
    {
        battlefieldManager = GetComponent<BattlefieldManager>();
        uIManager = GetComponent<UIManager>();
        
        SetupDataFor_DeployStage();
    }

    private void Awake()
    {
        gameStage = GameStages.Start;
        

    }





    // Update is called once per frame
    void Update()
    {

    }

    private void SetupDataFor_DeployStage()
    {
        initialUnitsSetup = GetComponent<InitialUnitsSetup>();
        List<UnitModel> PlayerUnits = initialUnitsSetup.PlayerInitialSetup();
        List<UnitModel> EnemyUnits = initialUnitsSetup.EnemyInitialSetup();

        //Actual call for Stage Update
        gameStage = GameStages.PlayerDeploy;
        StageUpdate_PlayerDeploy(PlayerUnits, EnemyUnits);
    }

    private void StageUpdate_PlayerDeploy(List<UnitModel> PlayerUnits, List<UnitModel> EnemyUnits)
    {
        //Prepare and Show UI
        uIManager.Battlefield_UnitsDeploy_Canvas(PlayerUnits);
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

        if( gameStage == GameStages.PlayerDeploy_UnitSelected)
        {
            if (newSelectedObject.tag == "Grid")
            {
                gameStage = GameStages.PlayerDeploy_UnitSelected_CellSelected;
                newSelectedObject.TryGetComponent<GridCell>(out GridCell gridCell);
                if (gridCell != null)
                {
                    var isSuccess = battlefieldManager.TryAddUnit(gridCell);
                    if(isSuccess)
                    {
                        gameStage = GameStages.PlayerDeploy;
                    }
                }
            }
        }

        lastSelectedObject = newSelectedObject;
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

    public void ClickOnCardDeploy(DeployCardController deployCardController)
    {
        gameStage = GameStages.PlayerDeploy_UnitSelected;
        Debug.Log($"Card {deployCardController.unitsInitialData.name} was clicked");
        battlefieldManager.SetUnitToCreate(deployCardController);
    }




    public enum GameStages
    {
        Start = 0 ,
        PlayerDeploy = 10 ,
        PlayerDeploy_UnitSelected = 11,
        PlayerDeploy_UnitSelected_CellSelected = 12,
        EnemyDeploy = 20 ,
        PlayerTurn = 100 , 
        EnemyTurn = 200 ,

    }
}
