using Alteruna;
using UnityEditor.Callbacks;
using UnityEngine;

public class PaddleSync : AttributesSync
{
    [SynchronizableMethod]
    void Push(Vector3 pushDirection, float force)
    {
        //Rigidbody rb = GetComponent<Rigidbody>();
        RigidbodySynchronizable rbS = GetComponent<RigidbodySynchronizable>();
        rbS.velocity = pushDirection.normalized * force;
        rbS.ForceUpdate();
    }

    [SynchronizableMethod]
    void Move(Vector3 pushDirection, float force)
    {
        //Rigidbody rb = GetComponent<Rigidbody>();
        RigidbodySynchronizable rbS = GetComponent<RigidbodySynchronizable>();
        rbS.velocity = pushDirection * force;
        rbS.ForceUpdate();
    }
}
