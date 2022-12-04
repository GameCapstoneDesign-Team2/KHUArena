using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("������!");
            Destroy(gameObject, 0);
        }
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject, 3);
        }
    }
}
