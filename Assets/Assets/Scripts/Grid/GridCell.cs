using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour , IMouseActionable
{
    public bool isOffset = false;
    public bool isSelected = false;
    public int visited = 1;
    public int x = 0;
    public int y = 0;
    public Material Normal, Offset, Highligh;
    public bool isOccupied = false;
    public Unit unit;

    public bool IsSelected => isSelected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(bool isOffset)
    {
        this.isOffset = isOffset;
        this.GetComponent<MeshRenderer>().material = isOffset ? Offset : Normal;
    }

    public void Highlight()
    {
        this.GetComponent<MeshRenderer>().material = Highligh;
    }

    public void UnHighlight()
    {
        this.GetComponent<MeshRenderer>().material = isOffset ? Offset : Normal;
    }

    public void Select()
    {
        if (this.unit != null)
        {
            unit.GetComponent<Unit>().SelectByPass();
        }
        else
        {
            this.isSelected = true;
            this.Highlight();
        }
    }

    public void ShowStepOnMap()
    {
        
    }

    public bool TryOcuppieCell()
    {
        if (isOccupied)
            return false;
        isOccupied = true;
        return true;
    }


    public void Deselect()
    {
        isSelected = false;
        this.UnHighlight();
        if (this.unit != null)
            unit.GetComponent<Unit>().Deselect();
    }

    public void Hover()
    {
        this.Highlight();
    }

    public void UnHover()
    {
        if(!isSelected)
            this.UnHighlight();
    }

    internal void RemoveUnit(Unit unit)
    {
        if (this.unit == unit)
        { 
            this.unit = null;
            this.isOccupied = false;
        }
    }

    internal void OcuppyNewUnit(Unit unit)
    {
        this.unit = unit;
        this.isOccupied = true;
    }

    public void Action()
    {
        if(unit != null)
        {
            unit.Action();
        }
        else
        {
            DoTheAction();
        }
    }

    private void DoTheAction()
    {
        BattlefieldManager.instance.MoveToSelectedCell(this);
    }
}
