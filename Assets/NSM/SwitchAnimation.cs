using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimation : MonoBehaviour
{
    private bool anim_mod = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Switch_Anim();
        }

    }

    void Switch_Anim()
    {
        Animator anim = GetComponent<Animator>();
        PlayerController playerController = GetComponent<PlayerController>();
        GetRotationMul getRotationMul = GetComponent<GetRotationMul>();

        if (anim_mod == false)
        {
            anim.enabled = true;
            playerController.enabled = true;
            getRotationMul.enabled = false;

            anim_mod = true;
        }
        else
        {
            anim.enabled = false;
            playerController.enabled = false;
            getRotationMul.enabled = true;

            anim_mod = false;

        }
    }
}
