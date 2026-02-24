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
        gameStage = GameStages.Start;
        SetupDataFor_DeployStage();
    }

    private void Awake()
    {
        uIManager = GetComponent<UIManager>();

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
        newSelectedObject.TryGetComponent<IMouseActionable>(out IMouseActionable actionable);

        if (gameStage == GameStages.PlayerDeploy)
        {
            if (lastActionable != null)
                lastActionable.Deselect();
            if (actionable != null)
                actionable.Select();    
        }

        lastSelectedObject = newSelectedObject;
    }

    public void ClickOnCardDeploy(UnitData unitData)
    {
        gameStage = GameStages.PlayerDeploy_UnitSelected;
        
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
