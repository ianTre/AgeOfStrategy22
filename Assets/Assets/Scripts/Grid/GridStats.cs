using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStats : MonoBehaviour , IMouseActionable
{
    public bool isOffset = false;
    public bool isSelected = false;
    public int visited = 1;
    public int x = 0;
    public int y = 0;
    public Material Normal, Offset, Highligh;

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
        isSelected = true;
        this.Highlight();
    }


    public void Deselect()
    {
        isSelected = false;
        this.UnHighlight();
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
}
