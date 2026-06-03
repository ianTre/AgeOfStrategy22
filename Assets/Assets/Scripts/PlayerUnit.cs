using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class PlayerUnit : MonoBehaviour, IMouseActionable
{
    private GameObject selectionLight;
    public bool hasBeingSelected = false;

    public void Deselect()
    {
        if (selectionLight != null)
            Destroy(selectionLight);
        this.hasBeingSelected = false;
    }

    public void Select()
    {
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
