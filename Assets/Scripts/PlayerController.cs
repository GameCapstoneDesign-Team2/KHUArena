using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private KeyCode jumpKeyCode = KeyCode.Space;

    [SerializeField]
    private Transform cameraTransform;

    private Movement3D movement3D;
    private PlayerAnimator playerAnimator;

    Vector3 moveVector;
    Vector3 dodgeVector;

    float x;
    float z;

    private float attackCoolTime = 0.5f;
    private float dodgeCoolTime = 3.0f;
    private float currentAttackTime = 0.5f;
    private float currentDodgeTime = 3.0f;

    private bool isAttackReady = true;
    private bool isDodgeReady = true;
    private bool isJump;
    private bool isDodge;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        movement3D = GetComponent<Movement3D>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    private void Update()
    {
        // ����Ű�� ���� �̵�
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        Move();
        Jump();
        Attack();
        Dodge();
    }

    void Attack()
    {
        currentAttackTime += Time.deltaTime;
        isAttackReady = attackCoolTime < currentAttackTime;

        // Sword ����
        if (Input.GetMouseButtonDown(0) && !isJump && !isDodge)
        {
            if (isAttackReady)
            {
                playerAnimator.OnWeaponAttack();
                currentAttackTime = 0;
            }
        }

        // Shield ����
        if (Input.GetMouseButton(1) && !isJump && !isDodge)
        {
            playerAnimator.OnShield();

            if (Input.GetKeyDown(KeyCode.R))
            {
                playerAnimator.animator.SetTrigger("isAttacked");
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            playerAnimator.OffShield();
        }
    }

    void Move()
    {
        moveVector = new Vector3(x, 0, z).normalized;

        if (isDodge)
        {
            moveVector = dodgeVector;
        }

        if (!isAttackReady)
        {
            moveVector *= 0.5f;
        }

        // �ִϸ��̼� �Ķ���� ����
        playerAnimator.OnMovement(moveVector.x, moveVector.z);

        // �̵� �Լ� ȣ�� (ī�޶� ���� �ִ� ������ �������� ����Ű�� ���� �̵�)
        movement3D.Moveto(cameraTransform.rotation * moveVector);

        // ȸ�� ���� (�׻� �ո� ������ ĳ������ ȸ���� ī�޶�� ���� ȸ�� ������ ����)
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
    }

    void Jump()
    {
        if (Input.GetKeyDown(jumpKeyCode))
        {
            if (!isJump && !isDodge)
            {
                movement3D.JumpTo(); // ���� �Լ� ȣ��
                playerAnimator.OnJump(); // �ִϸ��̼� �Ķ���� ���� (onJump)
                isJump = true;

                Invoke("JumpOut", 0.6f);
            }
        }
    }

    void JumpOut()
    {
        isJump = false;
    }

    void Dodge()
    {
        currentDodgeTime += Time.deltaTime;
        isDodgeReady = dodgeCoolTime < currentDodgeTime;

        if (isDodgeReady)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && moveVector != Vector3.zero && !isJump)
            {
                dodgeVector = moveVector;
                movement3D.MoveSpeed *= 2;
                playerAnimator.OnDodge();
                isDodge = true;

                Invoke("DodgeOut", 0.7f);
                currentDodgeTime = 0;
            }
        }
    }

    void DodgeOut()
    {
        movement3D.MoveSpeed *= 0.5f;
        isDodge = false;
    }
}
