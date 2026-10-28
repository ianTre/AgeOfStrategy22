using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsUIDisplayController : MonoBehaviour
{
    [SerializeField] GameObject deleteButton;

    internal GameObject ActivateDeleteButton()
    {
        deleteButton.SetActive(true);
        return deleteButton;
    }

    internal void DeactivateDeleteButton()
    {
        deleteButton.SetActive(false);
    }
}
