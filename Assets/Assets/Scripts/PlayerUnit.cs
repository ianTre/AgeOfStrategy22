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
        GameManager.instance.StageUpdate_PlayerTurn_UnitSelected(this.transform.GetComponent<Unit>());
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
