using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : LivingEntity
{
    [SerializeField]
    private Transform cameraTransform;

    private CharacterController characterController;
    public Animator playerAnimator;
    private Rigidbody rigidbody;

    Vector3 moveVector;
    Vector3 dodgeVector;

    float x;
    float z;

    public float moveSpeed;

    private float attackCoolTime = 0.8f;
    private float dodgeCoolTime = 3.0f;
    [SerializeField]
    private float currentAttackTime = 0.8f;
    private float currentDodgeTime = 3.0f;

    private bool isAttackReady = true;
    private bool isDodgeReady = true;
    private bool isJump;
    private bool isDodge;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        Move();
        Jump();
        Attack();
        Shield();
        Dodge();
    }

    private void FixedUpdate()
    {
        FreezeRotation();
    }

    void FreezeRotation()
    {
        rigidbody.angularVelocity = Vector3.zero;
    }

    void Attack()
    {
        currentAttackTime += Time.deltaTime;
        isAttackReady = attackCoolTime < currentAttackTime;

        // Sword 공격
        if (Input.GetMouseButtonDown(0) && isAttackReady && !isDodge)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
            {
                playerAnimator.SetLayerWeight(1, 1);
                playerAnimator.SetTrigger("onWeaponAttack");
            }
            currentAttackTime = 0;
        }
    }

    void Shield()
    {
        if (Input.GetMouseButton(1) && !isJump && !isDodge)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
            {
                playerAnimator.SetLayerWeight(1, 1);
                playerAnimator.SetBool("isShield", true);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                playerAnimator.SetTrigger("onShield");
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
            {
                playerAnimator.SetLayerWeight(1, 1);
                playerAnimator.SetBool("isShield", false);
            }
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

        playerAnimator.SetFloat("horizontal", -moveVector.x);
        playerAnimator.SetFloat("vertical", moveVector.z);
        playerAnimator.SetBool("isMove", (moveVector.x != 0.0f || moveVector.z != 0.0f));

        // characterController.Move(moveVector * moveSpeed * Time.deltaTime);

        // 이동 함수 호출 (카메라가 보고 있는 방향을 기준으로 방향키에 따라 이동)
        // movement3D.Moveto(cameraTransform.rotation * moveVector);

        Vector3 cameraRotation = cameraTransform.rotation * moveVector;
        Vector3 cameraYaw = new Vector3(cameraRotation.x, 0.0f, cameraRotation.z);

        transform.position += cameraYaw * moveSpeed * Time.deltaTime;

        // 회전 설정 (항상 앞만 보도록 캐릭터의 회전은 카메라와 같은 회전 값으로 설정)
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJump && !isDodge)
            {
                rigidbody.AddForce(Vector3.up * 8, ForceMode.Impulse);
                playerAnimator.SetBool("isJump", true);
                playerAnimator.SetTrigger("onJump");
                isJump = true;
            }
        }
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
                moveSpeed *= 2;
                playerAnimator.SetTrigger("onDodge");
                isDodge = true;

                Invoke("DodgeOut", 0.7f);
                currentDodgeTime = 0;
            }
        }
    }

    void DodgeOut()
    {
        moveSpeed *= 0.5f;
        isDodge = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerAnimator.SetBool("isJump", false);
            isJump = false;
        }
    }
}