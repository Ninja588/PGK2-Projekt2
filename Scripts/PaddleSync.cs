using Alteruna;
using UnityEngine;

public class PaddleSync : AttributesSync
{
    [SynchronizableMethod]
    void Push(Vector3 pushDirection, float force)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        RigidbodySynchronizable rbS = GetComponent<RigidbodySynchronizable>();
        rb.linearVelocity = pushDirection.normalized * force;
        rbS.ForceUpdate();
    }

    [SynchronizableMethod]
    void Move(Vector3 pushDirection, float force)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        RigidbodySynchronizable rbS = GetComponent<RigidbodySynchronizable>();
        rb.linearVelocity = pushDirection * force;
        rbS.ForceUpdate();
    }
}
