using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : LivingEntity
{
    [SerializeField]
    private KeyCode jumpKeyCode = KeyCode.Space;
    [SerializeField]
    private Transform cameraTransform;
    private Movement3D movement3D;
    private PlayerAnimator playerAnimator;

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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // �ִϸ��̼� �Ķ���� ����
        playerAnimator.OnMovement(x, z);

        // �̵� �ӵ� ���� (������ �̵��� ���� 5, �������� 2)
        movement3D.MoveSpeed = z > 0 ? 5.0f : 2.0f;

        // �̵� �Լ� ȣ�� (ī�޶� ���� �ִ� ������ �������� ����Ű�� ���� �̵�)
        movement3D.Moveto(cameraTransform.rotation * new Vector3(x, 0, z));


        // ȸ�� ���� (�׻� �ո� ������ ĳ������ ȸ���� ī�޶�� ���� ȸ�� ������ ����)
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);


        // space ������ ���� 
        if(Input.GetKeyDown(jumpKeyCode))
        {
            playerAnimator.OnJump(); // �ִϸ��̼� �Ķ���� ���� (onJump)
            movement3D.JumpTo(); // ���� �Լ� ȣ��
        }

        if(Input.GetMouseButtonDown(0))
        {
            //�ӵ� 0���� ������ٰ�
            playerAnimator.OnWeaponAttack();
            //GetMouseButtonUp
            //�ӵ� ���� 
        }
        
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerAnimator.OnShield();
        }
    }
}
