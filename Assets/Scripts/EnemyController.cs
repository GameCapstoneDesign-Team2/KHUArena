using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private SkinnedMeshRenderer meshRenderer;
    private Color originColor;
    public Transform target;

    public int maxHP;
    public int curHP;
    private float distance;
    public bool dead { get; protected set; }

    NavMeshAgent nav;
    Rigidbody rigid;
    CapsuleCollider capsuleCollider;

    //Ÿ���� �����ϴ��� �˷��ִ� �Լ�
    private bool hasTarget
    {
        get
        {
            if (target != null)
            {
                return true;
            }
            return false;
        }
    }

    private bool isWalk;
    private bool isAttack;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = meshRenderer.material.color;
        nav = GetComponent<NavMeshAgent>();
        

        //Ÿ�� : �÷��̾� 
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Update()
    {
        animator.SetBool("IsWalk", isWalk);
        animator.SetBool("IsAttack", isAttack);

        if (hasTarget)
        {
            distance = Vector3.Distance(target.position, target.transform.position);
        }

        

    }

    public void Attack()
    {
        this.transform.LookAt(target.transform);

        isAttack = true;
    }
    

    public void TakeDamage(int damage)
    {
        Debug.Log(damage + "�� ü���� �����մϴ�.");

        animator.SetTrigger("onHit");

        StartCoroutine("OnHitColor");
    }

    private IEnumerator OnHitColor()
    {
        meshRenderer.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        meshRenderer.material.color = originColor;

    }

    
    
}
