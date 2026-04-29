using Assets.Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static GameManager;

public class Unit : MonoBehaviour , ISoldier
{
    public UnitData data;
    
    public int number;
    public GridCell cell;
    public List<GridCell> path;
    [SerializeField] private float moveSpeed = 3f;              // velocidad en unidades por segundo
    [SerializeField] private float rotationSpeed = 5f;          // grados por segundo para rotar suavemente
    [SerializeField] private float stoppingDistance = 0.05f;    // distancia m�nima 


    public void InitUnit(UnitData data, GridCell cell)
    {
        this.data = data;
        this.cell = cell;
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
        if (GetComponent<UnitAnimationController>() == null)
        {
            Debug.LogError("Unit Animation Controller is not assigned in the prefab, please assign it to avoid this error");
        }
        GetComponent<UnitAnimationController>().TriggerRunning();

        foreach (var gridCell in path)
        {
            if (gridCell == null)
                continue;

            Vector3 target = gridCell.transform.position;
            // Smooth rotation towards target
            Quaternion targetRot = Quaternion.LookRotation(target - transform.position);
            float angleThreshold = 1f; // degrees
            while (Vector3.SqrMagnitude(transform.position - target) > stoppingDistance * stoppingDistance ||
                   Quaternion.Angle(transform.rotation, targetRot) > angleThreshold)
            {
                // Rotate towards target
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
                // Move towards target
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }
            // Ensure final position and rotation
            transform.position = target;
            transform.rotation = targetRot;
            yield return null;


        }
        GetComponent<UnitAnimationController>().TriggerIdle();
        GameManager.instance.gameStage = GameStages.EnemyTurn;
    }


    public void Action()
    {
        BattlefieldManager.instance.SelectTargetUnit(this);
    }

    public void Attack(ISoldier targetSoldier)
    {

    }

    public void ReceiveDamage(float damageAmmount)
    {
        throw new System.NotImplementedException();
    }
}
