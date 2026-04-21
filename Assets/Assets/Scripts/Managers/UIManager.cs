using Assets.Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform fatherPanel;
    List<Transform> panels;
    void Awake()
    {
        panels = new List<Transform>();
        for (int i = 0; i < fatherPanel.childCount; i++)
        {
            Transform child = fatherPanel.GetChild(i);
            if (child.CompareTag("UnitPanel"))
            {
                panels.Add(child);
                child.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Battlefield_UnitsDeploy_Canvas(List<UnitModel> PlayerUnits)
    {
        int index = 0;
        foreach (UnitModel PlayerUnit in PlayerUnits)
        {
            if (panels.Count < index + 1)
                throw new System.Exception("Not enough panels created for all units");
            panels[index].GetComponent<DeployCardController>().ShowUnitData(PlayerUnit.InitialData, PlayerUnit.number);
            index++;
        }
    }
}
