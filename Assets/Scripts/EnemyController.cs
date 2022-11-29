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
    private Transform _transform;
    private Transform playerTransform;

    public LivingEntity target; 

    //component
    NavMeshAgent nav;
    Rigidbody rigid;
    CapsuleCollider capsuleCollider;

    private bool isChase;
    private bool isWalk;
    private bool isAttack;
    private bool block;
    private bool isDead = false;

    public enum CurrentState { idle, chase, attack, dead };
    public CurrentState curState = CurrentState.idle;

    public float chaseDistance = 10.0f;
    public float attackDistance = 3f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = meshRenderer.material.color;
    }

    public void Setup(float newHealth)
    {
        startingHealth = newHealth;
        health = newHealth;
    }

    private void Start()
    {
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nav = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }

    /*
    void Update()
    {
        if(nav.enabled)
        {
            nav.SetDestination(target.transform.position);
            animator.SetBool("IsWalk", true);
        }
    }
    */

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

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);
            float distance = Vector3.Distance(playerTransform.position, _transform.position);

            if (distance <= attackDistance)
            {
                curState = CurrentState.attack;
            }
            else if (distance <= chaseDistance)
            {
                curState = CurrentState.chase;
            }
            else
            {
                curState = CurrentState.idle;
            }
        }
    }

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (curState)
            {
                case CurrentState.idle:
                    nav.Stop();
                    animator.SetBool("IsWalk", false);
                    break;
                case CurrentState.chase:
                    nav.destination = playerTransform.position;
                    nav.Resume();
                    animator.SetBool("IsWalk", true);
                    break;
                case CurrentState.attack:
                    nav.Stop();
                    animator.SetBool("IsAttack", true);
                    break;
            }

            yield return null;
        }
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
