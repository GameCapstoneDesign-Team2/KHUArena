using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;

    public Animator playerAnimator;
    private Rigidbody rigidbody;

    Vector3 moveVector;
    Vector3 dodgeVector;

    float x;
    float z;

    public float moveSpeed;

    private float attackCoolTime = 0.2f;
    private float dodgeCoolTime = 3.0f;
    [SerializeField]
    private float currentAttackTime = 0.2f;
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

        // Sword ����
        if (Input.GetMouseButtonDown(0) && !isJump && !isDodge)
        {
            if (isAttackReady)
            {
                if (playerAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
                {
                    playerAnimator.SetLayerWeight(1, 1);
                    playerAnimator.SetTrigger("onWeaponAttack");
                }
                currentAttackTime = 0;
            }
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

        // �̵� �Լ� ȣ�� (ī�޶� ���� �ִ� ������ �������� ����Ű�� ���� �̵�)
        // movement3D.Moveto(cameraTransform.rotation * moveVector);

        Vector3 cameraRotation = cameraTransform.rotation * moveVector;
        Vector3 cameraYaw = new Vector3(cameraRotation.x, 0.0f, cameraRotation.z);
 
        transform.position += cameraYaw * moveSpeed * Time.deltaTime;

        // ȸ�� ���� (�׻� �ո� ������ ĳ������ ȸ���� ī�޶�� ���� ȸ�� ������ ����)
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJump && !isDodge)
            {
                rigidbody.AddForce(Vector3.up * 10, ForceMode.Impulse);
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