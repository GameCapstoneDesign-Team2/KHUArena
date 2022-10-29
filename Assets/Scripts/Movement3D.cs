using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpForce = 3.0f;

    private Vector3 moveDirection;

    private CharacterController characterController;
    public float MoveSpeed
    {
        //�̵� �ӵ��� 2~5 ������ ���� ���� ����
        set => moveSpeed = Mathf.Clamp(value, 2.0f, 5.0f);
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
        if(characterController.isGrounded == false)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        // �̵� ����. CharacterController�� move() �Լ��� �̿��� �̵�
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
    
    public void Moveto(Vector3 direction)
    {
        moveDirection = new Vector3(direction.x, moveDirection.y, direction.z);
    }
    
    public void JumpTo()
    {
        //ĳ���Ͱ� �ٴ��� ��� ������ ����
        if (characterController.isGrounded == true)
        {
            moveDirection.y = jumpForce;
        }
    }
}
