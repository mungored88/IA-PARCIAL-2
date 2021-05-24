using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaLoca_Explotar : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<BombaLoca>().explosion.Play(0);
        Destroy(animator.gameObject, 1.5f);
    }
  
}
