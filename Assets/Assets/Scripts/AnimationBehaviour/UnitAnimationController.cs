using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAnimationController : MonoBehaviour
{
    private Animator anim;
    public string IdleAnimationLongStateName = "IdleStanding";
    public string IdleAnimationShortStateName = "IdleHolding";
    // The target number of loops
    public int targetLoopCount = 3;
    public bool hasChangedToIdle = false;
    /*public bool triggerRunning = false;
    public bool triggerAttack = false;
    public bool triggerDefend = false;
    public bool triggerDeath = false;*/

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }


    // Update is called once per frame
    void Update()
    {
        /*if (hasChangedToIdle && anim.GetCurrentAnimatorStateInfo(0).IsName(IdleAnimationLongStateName))
            StartCoroutine(CheckAnimationLoops(Random.Range(1, 6)));*/
    }

    public void TriggerIdle()
    {
        anim.SetTrigger("IdleHoldingTrigger");
    }

    public void TriggerRunning()
    {
        anim.SetTrigger("RunningTrigger");
    }

    public void TriggerAttack()
    {
        anim.SetTrigger("AttackTrigger");
    }

    public void TriggerDefend()
    {
        anim.SetTrigger("DefenseTrigger");
    }
    
    public void TriggerDeath()
    {
        anim.SetTrigger("DeathTrigger");
    }


    /*IEnumerator CheckAnimationLoops(int nLoops)
    {
        hasChangedToIdle = false;
        //Debug.Log("Will loop for " + nLoops + " times.");
        // Wait until the animation state is playing
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(IdleAnimationLongStateName))
        {
            yield return null;
        }

        int currentLoop = 0;
        while (nLoops > 0)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            // The integer part of normalizedTime is the number of loops completed
            int loopsCompleted = (int)Mathf.Floor(stateInfo.normalizedTime);

            if (loopsCompleted > currentLoop)
            {
                currentLoop = loopsCompleted;
                nLoops--;
                //Debug.Log("Loop completed. Remaining loops: " + nLoops);
            }

            if (nLoops == 0)
            {
                //Debug.Log("Target loops reached. Transitioning to next state/stopping.");
                currentLoop = 0; // Reset loop count if you want to check for the next state
                anim.SetTrigger("IdleHoldingTrigger");
                hasChangedToIdle = true;
                break;
            }

            yield return null;
        }
    }*/

}
