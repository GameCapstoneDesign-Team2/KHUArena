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

    public enum CurrentState { idle, walk, attack, dead, block, onHit};
    public CurrentState curState = CurrentState.idle;

    public float chaseDistance;
    public float attackDistance;
    public float shieldDistance;

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

    public IEnumerator CheckState()
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
            else if (startingHealth == 0)
            {
                animator.SetTrigger("Dead");
                nav.isStopped = true;
                isDead = true;
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
        float distance = Vector3.Distance(playerTransform.position, _transform.position);

        nav.ResetPath();
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("IsWalk", false);

        if (distance <= shieldDistance)
        {
            //animator.SetBool("IsWalk", false);
            animator.SetBool("Block", true);
            animator.SetBool("IsAttack", false);
        }
        else if (distance > shieldDistance)
        {
            //animator.SetBool("IsWalk", false);
            animator.SetBool("IsAttack", true);
            animator.SetBool("Block", false);
        }
        
    }

    
    
}
