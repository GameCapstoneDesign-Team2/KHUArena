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
        // 방향키를 눌러 이동
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 애니메이션 파라미터 설정
        playerAnimator.OnMovement(x, z);

        // 이동 속도 설정 (앞으로 이동할 때만 5, 나머지는 2)
        movement3D.MoveSpeed = z > 0 ? 5.0f : 2.0f;

        // 이동 함수 호출 (카메라가 보고 있는 방향을 기준으로 방향키에 따라 이동)
        movement3D.Moveto(cameraTransform.rotation * new Vector3(x, 0, z));


        // 회전 설정 (항상 앞만 보도록 캐릭터의 회전은 카메라와 같은 회전 값으로 설정)
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);


        // space 누르면 점프 
        if(Input.GetKeyDown(jumpKeyCode))
        {
            playerAnimator.OnJump(); // 애니메이션 파라미터 설정 (onJump)
            movement3D.JumpTo(); // 점프 함수 호출
        }

        if(Input.GetMouseButtonDown(0))
        {
            //속도 0으로 만들었다가
            playerAnimator.OnWeaponAttack();
            //GetMouseButtonUp
            //속도 복구 
        }
        
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerAnimator.OnShield();
        }
    }
}
