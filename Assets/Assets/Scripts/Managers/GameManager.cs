using Assets.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameStages gameStage;
    private InitialUnitsSetup initialUnitsSetup;

    void Start()
    {
        gameStage = GameStages.Start;
    }

    private void Awake()
    {
        SetupDataFor_DeployStage();
    }

    private void SetupDataFor_DeployStage()
    {
        initialUnitsSetup = GetComponent<InitialUnitsSetup>();
        List<UnitModel> PlayerUnits = CreateListPlayerUnits();
        List<UnitModel> EnemyUnits = CreateListEnemyUnits();
        StageUpdate_PlayerDeploy(PlayerUnits, EnemyUnits);
    }

    /// <summary>
    /// Retrieves a list of all player-controlled units. This method will CREATE the list of units in the battlefield-only plan of the game and will need to be replaced to GetListPlayerUnits() to GET data if we moved to CAMPAING plan.
    /// </summary>
    /// <returns>A list of <see cref="UnitModel"/> objects representing the player's units.  The list will be empty if the player
    /// has no units.</returns>
    /// <exception cref="NotImplementedException">Thrown if the method is not yet implemented.</exception>
    private List<UnitModel> CreateListPlayerUnits()
    {
        var initialUnits = initialUnitsSetup.PlayerInitialSetup();
        var playerUnits = new List<UnitModel>();
        foreach (var unit in initialUnits)
        {
            playerUnits.Add(new UnitModel(unit.Item1 , unit.Item2 ));
        }
        return playerUnits;
    }

    private List<UnitModel> CreateListEnemyUnits()
    {
        var initialUnits = initialUnitsSetup.PlayerInitialSetup();
        var playerUnits = new List<UnitModel>();
        foreach (var unit in initialUnits)
        {
            playerUnits.Add(new UnitModel(unit.Item1, unit.Item2));
        }
        return playerUnits;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StageUpdate_PlayerDeploy(List<UnitModel> PlayerUnits , List<UnitModel> EnemyUnits)
    {
        
    }



    public enum GameStages
    {
        Start = 0 ,
        PlayerDeploy = 10 , 
        EnemyDeploy = 20 ,
        PlayerTurn = 100 , 
        EnemyTurn = 200 ,

    }
}
