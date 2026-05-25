using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseActionable
{
    public void Select();
    public void Deselect();
    public void Action();
}

public interface IFocusable
{
    public void Hover();
    public void UnHover();
}
