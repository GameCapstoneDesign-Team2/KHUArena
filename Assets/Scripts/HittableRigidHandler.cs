using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableRigidHandler : MonoBehaviour
{
    private List<Vector3> _collisionPoints = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void CollectCollisionPoint(Vector3 point)
    {
        _collisionPoints.Add(point);
    }
    
    public void ClearCollisionList()
    {
        _collisionPoints.Clear();
    }
}
