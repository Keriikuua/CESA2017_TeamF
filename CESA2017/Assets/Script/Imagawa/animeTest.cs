using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animeTest : MonoBehaviour {

    Animator animator;
    bool attackflg = false;
    bool waiteflg = false;

    private void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        animator.SetBool("AttackFlg", false);
        animator.SetBool("WaiteFlg", false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!attackflg)
            {
                animator.SetBool("AttackFlg", true);
                attackflg = true;
            }
            else
            {
                animator.SetBool("AttackFlg", false);
                attackflg = false;
            }

            Debug.Log(animator.GetBool("AttackFlg"));
            Debug.Log(attackflg);
        }


        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!waiteflg)
            {
                animator.SetBool("WaiteFlg", true);
                waiteflg = true;
            }
            else
            { 
                animator.SetBool("WaiteFlg", false);
                waiteflg = false;
            }
            Debug.Log(animator.GetBool("WaiteFlg"));
            Debug.Log(waiteflg);
        }
    }

}
