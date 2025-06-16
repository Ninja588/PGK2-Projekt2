using Alteruna;
using UnityEngine;

public class PaddleSync : AttributesSync
{
    [SynchronizableMethod]
    void Push(Vector3 pushDirection, float force)
    {
        RigidbodySynchronizable rbS = GetComponent<RigidbodySynchronizable>();
        rbS.velocity = pushDirection.normalized * force;
        rbS.ForceUpdate();
    }

    [SynchronizableMethod]
    void Move(Vector3 pushDirection, float force)
    {
        RigidbodySynchronizable rbS = GetComponent<RigidbodySynchronizable>();
        rbS.velocity = pushDirection * force;
        rbS.ForceUpdate();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                Vector3 awayFromPaddle = (other.transform.position - transform.position).normalized;
                pm.ApplyPush(awayFromPaddle, 50.0f);
            }
        }
    }

}
