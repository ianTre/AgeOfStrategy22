using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class PlayerUnit : MonoBehaviour, IMouseSelectable
{
    private GameObject selectionLight;
    public bool hasBeingSelected = false;
    Unit unit;

    public void Start()
    {
        unit = this.transform.GetComponent<Unit>();
    }

    public void Deselect()
    {
        if (selectionLight != null)
            Destroy(selectionLight);
        this.hasBeingSelected = false;
    }

    public void Select()
    {
        bool canSelect = unit?.isAlive ?? false;
        if (!canSelect)
            return;
        if (GameManager.instance.StageUpdate_UnitSelected(this.transform.GetComponent<Unit>()))
        {
            CreateSelectionLight();
            this.hasBeingSelected = true;
        }
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
}
