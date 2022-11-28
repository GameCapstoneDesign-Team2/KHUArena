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
    public BoxCollider attackRange;

    private bool isChase;
    private bool isWalk;
    private bool isAttack;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = meshRenderer.material.color;
        nav = GetComponent<NavMeshAgent>();

        //2초 후 추적 시작
        Invoke("ChaseStart", 2);
    }

    public void Setup(float newHealth)
    {
        startingHealth = newHealth;
        health = newHealth;
    }

    void ChaseStart()
    {
        isChase = true;
        animator.SetBool("IsWalk", true);
    }

    void Update()
    {
        if(nav.enabled)
        {
            nav.SetDestination(target.transform.position);
            nav.isStopped = !isChase;
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

    void Attack()
    {
        isChase = false;
        isAttack = true;
        animator.SetBool("isAttack", true);
    }

    public void OnDamage()
    {   
        //맞으면 반격
        animator.SetTrigger("OnHit");
        Attack();
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
