using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static GameManager;

public class Unit : MonoBehaviour, IMouseActionable
{
    public UnitData data;
    public bool hasBeingSelected = false;
    public int number;
    public GridCell cell;
    public List<GridCell> path;
    private GameObject selectionLight;
    [SerializeField] private float moveSpeed = 3f;       // velocidad en unidades por segundo
    [SerializeField] private float stoppingDistance = 0.05f; // distancia mínima 

    public void Deselect()
    {
        if (selectionLight != null)
            Destroy(selectionLight);
        this.hasBeingSelected = false;
    }

    public void Hover()
    {

    }

    public void Select()
    {
        CreateSelectionLight();
        this.hasBeingSelected = true;
        BattlefieldManager.instance.SelectANewUnit(this);


        GameStages stage = GameManager.instance.gameStage;
        switch (stage)
        {
            case GameStages.Start:
            case GameStages.PlayerDeploy:
            case GameStages.PlayerDeploy_CardSelected:
            case GameStages.PlayerDeploy_CardSelected_CellSelected:
                break;
            case GameStages.EnemyDeploy:
                break;
            case GameStages.PlayerTurn:
                break;
            case GameStages.PlayerTurn_UnitSelected:
                break;
            case GameStages.PlayerTurn_UnitSelected_DestinySelected:
                break;
            case GameStages.EnemyTurn:
                break;
            default:
                break;
        }

    }

    public void SelectByPass()
    {
        Select();
    }

    public void UnHover()
    {

    }

    public void CreateSelectionLight()
    {
        if (!this.hasBeingSelected)
            selectionLight = Instantiate(BattlefieldManager.instance.selectionLightPrefab, this.transform.position, Quaternion.identity);
    }

    public void InitUnit(UnitData data, GridCell cell)
    {
        this.data = data;
        this.cell = cell;
        this.hasBeingSelected = false;
    }

    public void MoveToNewCell(GridCell newCell)
    {
        if (newCell.isOccupied)
            return;
        this.path = BattlefieldManager.instance.FindShortestPath(this.cell, newCell);
        if (path != null && path.Count > 0)
        {
            this.cell.RemoveUnit(this); //Clean Old Cell
            this.cell = newCell;
            this.cell.OcuppyNewUnit(this); //Assign new Cell
            StartCoroutine(MoveToNewPosition(path));
        }
    }

    public IEnumerator MoveToNewPosition(List<GridCell> path)
    {
        if (path == null || path.Count == 0)
            yield break;

        foreach (var gridCell in path)
        {
            if (gridCell == null)
                continue;

            Vector3 target = gridCell.transform.position;

            // Mover hasta la posición objetivo
            while (Vector3.SqrMagnitude(transform.position - target) > stoppingDistance * stoppingDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

                // Si hay una luz de selección, mantenerla sobre la unidad
                if (selectionLight != null)
                    selectionLight.transform.position = transform.position;

                yield return null; // esperar hasta el siguiente frame
            }

            // Asegurar posición exacta al llegar
            transform.position = target;
            if (selectionLight != null)
                selectionLight.transform.position = transform.position;

            yield return null;
        }
    }





    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Action()
    {
        throw new System.NotImplementedException();
    }
}
