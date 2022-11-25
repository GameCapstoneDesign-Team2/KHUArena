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
    public Transform transform;

    private float distance;

    public float damage = 20f;
    public float attackDelay = 1f;
    private float lastAttackTime;
    private float attackRange = 2.3f;

    private NavMeshAgent nav;
    Rigidbody rigid;
    CapsuleCollider capsuleCollider;

    private bool isWalk;
    private bool isAttack;

    private bool hasTarget
    {
        get
        {
            if (target != null && !target.dead)
            {
                return true;
            }

            return false;
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = meshRenderer.material.color;
        nav = GetComponent<NavMeshAgent>();
    }

    public void Setup(float newHealth, float newDamage, float newSpeed)
    {
        startingHealth = newHealth;
        health = newHealth;
        damage = newDamage;
        nav.speed = newSpeed;
    }

    private void Start()
    {
        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        animator.SetBool("IsWalk", isWalk);
        animator.SetBool("IsAttack", isAttack);

        if (hasTarget)
        {
            distance = Vector3.Distance(transform.position, target.transform.position);
        }
    }

    private IEnumerator UpdatePath()
    {
        while(!dead)
        {
            if (hasTarget) //추척 대상이 있으면 공격
            {
                Attack();
            }

            else
            {
                //추적 대상이 없으면 이동 정지
                nav.isStopped = true;
                isAttack = false;
                isWalk = false;
            }

            //0.25주기로 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void Attack()
    {
        if (!dead && distance < attackRange)
        {
            isWalk = false;

            this.transform.LookAt(target.transform); //타깃 바라보기

            if (lastAttackTime + attackDelay <= Time.time)
            {
                isAttack = true;
            }

            else
            {
                isAttack = false;
            }
        }

        else
        {
            isWalk = true;
            isAttack = false;
            //계속 추적
            nav.isStopped = false; //계속 이동
            nav.SetDestination(target.transform.position);
        }
    }

    public override void OnDamage(float damage)
    {
        animator.SetTrigger("OnHit");
        base.OnDamage(damage);
    }

    //데미지 적용시키기 
    public void OnDamageEvent()
    {
        LivingEntity attackTarget = target.GetComponent<LivingEntity>();

        attackTarget.OnDamage(damage);

        lastAttackTime = Time.time;
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
