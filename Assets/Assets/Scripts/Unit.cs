using Assets.Assets.Scripts.Interfaces;
using System;
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
    public bool isAlive = true;
    [SerializeField] private float moveSpeed = 3f;              // velocidad en unidades por segundo
    [SerializeField] private float rotationSpeed = 5f;          // grados por segundo para rotar suavemente
    [SerializeField] private float stoppingDistance = 0.05f;    // distancia m�nima 
    public int Id;


    public void InitUnit(UnitData data, GridCell cell, int amount)
    {
        this.data = data;
        this.cell = cell;
        this.number = amount;
        HealthBarController healthBarController = this.transform.GetComponentInChildren<HealthBarController>();
        healthBarController?.InitializeMaxHealth(data.health);
    }



    public IEnumerator MoveToNewPosition(List<GridCell> path,Action onComplete)
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
        onComplete?.Invoke();
    }

    public void IsAlive(bool alive)
    {
        this.isAlive = alive;
    }


    public void Action()
    {
        if (isAlive)
        {
            BattlefieldManager.instance.SelectTargetUnit(this);
        }
    }

    public void Attack(ISoldier targetSoldier)
    {

    }

    public void ReceiveDamage(float damageAmmount)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator AttackTarget(Unit target, Action onComplete)
    {
        yield return new WaitForSeconds(1.5f); // Simulate attack delay
        this.transform.LookAt(target.transform);
        target.transform.LookAt(this.transform);
        this.GetComponent<UnitAnimationController>().TriggerAttack();
        yield return new WaitForSeconds(0.3f); // Simulate attack delay
        target.GetComponent<UnitAnimationController>().TriggerDefend();
        target.GetComponentInChildren<HealthBarController>().DisplayDamage();
        if(!target.isAlive)
        {
            yield return new WaitForSeconds(0.5f); // Simulate attack delay
            target.GetComponent<UnitAnimationController>().TriggerDeath();
        }
        yield return new WaitForSeconds(1f); // Simulate attack delay
        onComplete?.Invoke();
    }
}
