using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseActionable
{
    public void Select();
    public void Deselect();
    public void Hover();
    public void UnHover();
}
