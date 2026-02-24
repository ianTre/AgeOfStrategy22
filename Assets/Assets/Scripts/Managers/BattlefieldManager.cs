using Assets.Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    List<UnitModel> PlayerUnits;
    List<UnitModel> EnemyUnits;
    private GridController gridController;
    private GameObject intendedToCreatePrefab;
    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        PlayerUnits = new List<UnitModel>();
        EnemyUnits = new List<UnitModel>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool TryAddUnit(UnitModel unit, int x, int y)
    {
        if (gridController.gridArray[x, y].GetComponent<GridCell>().isOccupied)
            return false;
        gridController.gridArray[x, y].GetComponent<GridCell>().isOccupied = true;
        Instantiate(unit.InitialData.prefab , gridController.gridArray[x, y].transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        PlayerUnits.Add(unit);
        return true;
    }
}
