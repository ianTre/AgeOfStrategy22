using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static GameManager;

public class PlayerUnit : MonoBehaviour, IMouseSelectable
{
    private GameObject selectionLight;
    public bool hasBeingSelected = false;
    Unit unit;
    private List<GameStages> deployStages;

    public void Start()
    {
        unit = this.transform.GetComponent<Unit>();
        deployStages = new List<GameStages> { GameStages.PlayerDeploy, GameStages.PlayerDeploy_CardSelected, GameStages.PlayerDeploy_CardSelected_CellSelected };
    }

    public void Deselect()
    {
        GameManager _gameManager = GameManager.instance;
        if (selectionLight != null)
            Destroy(selectionLight);

        if (_gameManager.CheckStages(deployStages))
            HideDeleteButton();

        this.hasBeingSelected = false;
    }

    public void Select()
    {
        GameManager _gameManager = GameManager.instance;
        bool canSelect = unit?.isAlive ?? false;
        if (!canSelect)
            return;

        //Deploy delete button (X) if the game is in the deploy stage
        if (_gameManager.CheckStages(deployStages))
            DisplayDeleteButton();
        
        if (_gameManager.StageUpdate_UnitSelected(this.transform.GetComponent<Unit>()))
        {
            CreateSelectionLight();
            
            this.hasBeingSelected = true;
        }
    }

    public void DisplayDeleteButton()
    {
        UnitsUIDisplayController UIController = GetUnitUIController();
        var deleteButton = UIController?.ActivateDeleteButton();
    }

    public void HideDeleteButton()
    {
        UnitsUIDisplayController UIController = GetUnitUIController();
        UIController?.DeactivateDeleteButton();
    }

    private UnitsUIDisplayController GetUnitUIController()
    {
        UnitsUIDisplayController uIController;
        var canvas = this.transform.Find("UnitsDisplayCanvas");
        if (canvas == null)
        {
            Debug.LogError("UnitsDisplayCanvas not found in the unit's hierarchy for unit" + this.name);
            return null;
        }
        uIController = canvas.GetComponent<UnitsUIDisplayController>();
        if (uIController == null)
        {
            Debug.LogError("UnitsUIDisplayController not found in the UnitsDisplayCanvas for unit" + this.name );
            return null;
        }
        return uIController;
    }

    public void SelectByPass()
    {
        Select();
    }

    public void CreateSelectionLight()
    {
        if (!this.hasBeingSelected)
            selectionLight = Instantiate(BattlefieldManager.instance.selectionLightPrefab, this.transform.position, Quaternion.identity, this.transform);
    }



    public void Action()
    {
        throw new System.NotImplementedException();
    }

    public GameObject GetDeleteButton()
    {
        var canvas = this.transform.Find("UnitsDisplayCanvas");
        if (canvas == null)
        {
            Debug.LogError("UnitsDisplayCanvas not found in the unit's hierarchy.");
            return null;
        }
        var deleteButton = canvas.transform.Find("RemoveButton").gameObject;
        if(deleteButton == null)
        {
            Debug.LogError("RemoveButton not found in the UnitsDisplayCanvas.");
            return null;
        }
        return deleteButton;
    }
}
