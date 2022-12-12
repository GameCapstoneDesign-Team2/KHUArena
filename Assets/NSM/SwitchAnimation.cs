using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimation : MonoBehaviour
{
    private bool anim_mod = false;

    private Animator anim;
    private PlayerController playerController;
    private GetRotationMul getRotationMul;

    private float currentAttackTime = 1.0f;
    private bool isAttackReady = true;
    private float attackCoolTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        getRotationMul = GetComponent<GetRotationMul>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Switch_Anim();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            Shield();
        }
    }
    void Shield()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
            {
                anim.SetLayerWeight(1, 1);
                anim.SetBool("isShield", true);
            }
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            anim.SetLayerWeight(1, 1);
            anim.SetBool("isShield", false);
        }
    }

    void Attack()
    {

        currentAttackTime += Time.deltaTime;
        isAttackReady = attackCoolTime < currentAttackTime;

        // Sword АјАн
        if (Input.GetMouseButtonDown(0) && isAttackReady)
        {
            anim.enabled = true;
            StopCoroutine("Swing");
            StartCoroutine("Swing");
            anim.SetLayerWeight(1, 1);
            anim.SetTrigger("onWeaponAttack");

            currentAttackTime = 0;
        }
    }
    IEnumerator Swing()
    {
        /*
        yield return new WaitForSeconds(0.2f);
        attackCollision.enabled = true;

        yield return new WaitForSeconds(0.5f);
        attackCollision.enabled = false;
        */
        yield return new WaitForSeconds(1f);
        anim.enabled = false;
    }

    void Switch_Anim()
    {

        if (anim_mod == false)
        {
            anim.enabled = true;
            playerController.enabled = true;
            getRotationMul.enabled = false;

            anim_mod = true;
        }
        else
        {
            anim.enabled = false;
            playerController.enabled = false;
            getRotationMul.enabled = true;

            anim_mod = false;

        }
    }
}
