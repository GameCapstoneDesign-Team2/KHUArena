using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : LivingEntity
{
    private Animator animator;
    private SkinnedMeshRenderer meshRenderer;
    private LayerMask whatIsTarget;
    private Color originColor;

    public LivingEntity target; 

    private float distance;

    //component
    NavMeshAgent nav;
    Rigidbody rigid;
    CapsuleCollider capsuleCollider;
    public BoxCollider attackRange; //공격 범위

    private bool isChase;
    private bool isWalk;
    private bool isAttack;
    private bool block;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = meshRenderer.material.color;
        nav = GetComponent<NavMeshAgent>();

        nav.isStopped = true;
        StartCoroutine(Attack());
    }

    public void Setup(float newHealth)
    {
        startingHealth = newHealth;
        health = newHealth;
    }

    void Update()
    {
        if(nav.enabled)
        {
            nav.SetDestination(target.transform.position);
            animator.SetBool("IsWalk", true);
            if()
        }
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 3);
        switch (ranAction)
        {
            case 0:
                StartCoroutine(ShieldAttack());
                break;
            case 1:
            case 2:
                StartCoroutine(Attack_1());
                break;
        }
    }

    IEnumerator Attack_1()
    {
        animator.SetBool("IsAttack", true);
        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(2.5f);
        StartCoroutine(Attack());
    }


    IEnumerator ShieldAttack()
    {
        animator.SetBool("Block", true);
        yield return new WaitForSeconds(3f);
        StartCoroutine(Attack());
    }

    private IEnumerator OnHitColor()
    {
        meshRenderer.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        meshRenderer.material.color = originColor;

    }

    public override void Die()
    {
        base.Die();

        //다른 AI를 방해하지 않도록 자신의 모든 콜라이더를 비활성화
        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        nav.isStopped = true;
        nav.enabled = false;

        animator.SetTrigger("Die");
    }

}
