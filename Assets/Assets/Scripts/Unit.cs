using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Unit : MonoBehaviour , IMouseActionable
{
    public UnitData data;
    private bool hasBeingSelected = false;
    public GridCell cell;
    private GameObject selectionLight;

    public void Deselect()
    {
        
    }

    public void Hover()
    {
        
    }

    public void Select()
    {
        this.cell.Select();
    }

    public void SelectByPass()
    {
        CreateSelectionLight();
        this.hasBeingSelected = true;
    }

    public void UnHover()
    {
        
    }

    public void CreateSelectionLight()
    {
        if (!this.hasBeingSelected)
            selectionLight = Instantiate(BattlefieldManager.instance.selectionLightPrefab, this.transform.position , Quaternion.identity);
    }

    public void InitUnit(UnitData data, GridCell cell)
    {
        this.data = data;
        this.cell = cell;
        this.hasBeingSelected = false;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
