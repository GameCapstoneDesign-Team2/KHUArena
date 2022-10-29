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

        //애니메이션 파라미터 설정
        playerAnimator.OnMovement(x, z);

        movement3D.MoveSpeed = z > 0 ? 5.0f : 2.0f;
        movement3D.Moveto(cameraTransform.rotation * new Vector3(x, 0, z));

        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        if(Input.GetKeyDown(jumpKeyCode))
        {
            playerAnimator.OnJump();
            movement3D.JumpTo();
        }

        if(Input.GetMouseButtonDown(0))
        {
            playerAnimator.OnWeaponAttack();
        }
    }
}
