using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Attack");
            Vector3 contactPoint = collision.ClosestPoint(transform.position);

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = Vector3.one * 0.5f;
            sphere.transform.position = contactPoint;
            sphere.transform.parent = collision.gameObject.transform;
            sphere.GetComponent<Collider>().enabled = false;
            Destroy(sphere, 3f);
        }
    }
}