using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject attackCollision;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMovement (float horizontal, float vertical)
    {
        animator.SetFloat("horizontal", -horizontal);
        animator.SetFloat("vertical", vertical);
    }

    public void OnJump()
    {
        animator.SetTrigger("onJump");
    }

    public void OnWeaponAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
        {
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("onWeaponAttack");
        }
    }

    public void OnShield()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
        {
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("onShield");
        }
    }

    public void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }
}
