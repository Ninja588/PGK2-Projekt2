using UnityEngine;
using Alteruna;
using System.Collections;

public class BallEnabler : MonoBehaviour
{
    [SerializeField] private BallScript ballScript;
    [SerializeField] private UserDisconnectHandler userDisconnectHandler;
    public bool playerWasHere = false;

    void Awake()
    {
        StartCoroutine(Check());
    }

    private IEnumerator Check()
    {
        while (true)
        {
            if (Multiplayer.Instance.GetUsers().Count == 2)
            {
                yield return new WaitForSeconds(4);
                ballScript.enabled = true;
                ballScript.StartBall();
                playerWasHere = true;
                if (!userDisconnectHandler.enabled) userDisconnectHandler.enabled = true;
                this.enabled = false;
                break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void StartCheck()
    {
        StartCoroutine(Check());
    }
}
