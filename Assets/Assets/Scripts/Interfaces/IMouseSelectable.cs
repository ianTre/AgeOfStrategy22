using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseSelectable
{
    public void Select();
    public void Deselect();

}

public interface IMouseActionable
{
    public void Action();
}

public interface IFocusable
{
    public void Hover();
    public void UnHover();
}
