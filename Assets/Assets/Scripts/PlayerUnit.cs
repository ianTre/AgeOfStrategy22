using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class PlayerUnit : MonoBehaviour , IMouseActionable
{
    private GameObject selectionLight;
    public bool hasBeingSelected = false;

    public void Deselect()
    {
        if (selectionLight != null)
            Destroy(selectionLight);
        this.hasBeingSelected = false;
    }

    public void Hover()
    {

    }

    public void Select()
    {
        CreateSelectionLight();
        this.hasBeingSelected = true;
        BattlefieldManager.instance.SelectANewUnit(this.transform.GetComponent<Unit>());


        GameStages stage = GameManager.instance.gameStage;
        switch (stage)
        {
            case GameStages.Start:
            case GameStages.PlayerDeploy:
            case GameStages.PlayerDeploy_CardSelected:
            case GameStages.PlayerDeploy_CardSelected_CellSelected:
                break;
            case GameStages.EnemyDeploy:
                break;
            case GameStages.PlayerTurn:
                break;
            case GameStages.PlayerTurn_UnitSelected:
                break;
            case GameStages.PlayerTurn_UnitSelected_DestinySelected:
                break;
            case GameStages.EnemyTurn:
                break;
            default:
                break;
        }

    }

    public void SelectByPass()
    {
        Select();
    }

    public void UnHover()
    {

    }

    public void CreateSelectionLight()
    {
        if (!this.hasBeingSelected)
            selectionLight = Instantiate(BattlefieldManager.instance.selectionLightPrefab, this.transform.position, Quaternion.identity,this.transform);
    }

    

    public void Action()
    {
        throw new System.NotImplementedException();
    }
}
