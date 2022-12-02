using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : LivingEntity
{
    private Animator animator;
    private SkinnedMeshRenderer meshRenderer;
    private LayerMask whatIsTarget;
    private Transform _transform;
    private Transform playerTransform;

    public LivingEntity target; 

    //component
    NavMeshAgent nav;
    Rigidbody rigid;
    CapsuleCollider capsuleCollider;

    private bool isDead = false;

    public enum CurrentState { idle, walk, attack, dead };
    public CurrentState curState = CurrentState.idle;

    public float chaseDistance = 9.0f;
    public float attackDistance = 3f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
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
    }

    void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    public void Chase(Vector3 targetPosition)
    {
        animator.SetBool("IsWalk", true);
        nav.SetDestination(targetPosition);
        
        animator.SetBool("IsAttack", false);
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
                StartCoroutine(Attack());
            }
            else if (distance <= chaseDistance)
            {
                curState = CurrentState.walk;
                Chase(playerTransform.position);
            }
            /*
            else
            {
                curState = CurrentState.idle;
                animator.SetBool("IsWalk", true);
                animator.SetBool("IsAttack", false);
            }
            */
        }
    }

    IEnumerator Attack()
    {
        nav.ResetPath();

        yield return new WaitForSeconds(0.3f);

        animator.SetBool("IsWalk", false);
        animator.SetBool("IsAttack", true);

        //StartCoroutine(CheckState());

    }

    /*
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
    */
}
