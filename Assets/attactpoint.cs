using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attactpoint : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "object")
        {
            ContactPoint contactPoint = collision.contacts[0];

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = Vector3.one * 0.5f;
            sphere.transform.position = contactPoint.point;
            sphere.transform.parent = transform.parent;
            sphere.GetComponent<Collider>().enabled = false;
            Destroy(sphere, 3f);
        }
    }
}
