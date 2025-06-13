using System.Collections;
using Alteruna;
using UnityEngine;

public class BallAbilitiesSync : AttributesSync
{
    private RigidbodySynchronizable rbS;

    void Start()
    {
        rbS = GetComponent<RigidbodySynchronizable>();
    }

    // slow
    [SynchronizableMethod]
    public void SlowBall()
    {
        if (!Multiplayer.Instance.GetUser().IsHost) return;
        Debug.Log("slow host");

        Vector3 temp = rbS.velocity;
        // Debug.Log("Rbs: " + rbS.velocity + "temp/2:" + temp/2);
        rbS.velocity = temp / 2;
        rbS.ForceUpdate();
        StartCoroutine(SlowCoolDown(temp));
    }
    private IEnumerator SlowCoolDown(Vector3 temp)
    {
        yield return new WaitForSeconds(4);
        rbS.velocity = -temp;
    }
    // boost
    [SynchronizableMethod]
    public void BoostBall()
    {
        if (!Multiplayer.Instance.GetUser().IsHost) return;
        // Debug.Log("boost host");
        
        rbS.velocity *= 1.5f;
        rbS.ForceUpdate();
    }
}
