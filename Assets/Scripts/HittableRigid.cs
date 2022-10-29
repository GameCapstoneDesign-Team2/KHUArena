using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableRigid : MonoBehaviour
{
    public HittableRigidHandler hittableRigidHandler;

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = new ContactPoint[collision.contactCount];
        collision.GetContacts(contactPoints);
        foreach (ContactPoint contactPoint in contactPoints)
        {
            hittableRigidHandler.CollectCollisionPoint(contactPoint.point);
        }
    }
}
