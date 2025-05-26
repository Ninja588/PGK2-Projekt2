using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Alteruna;

public class UserDisconnectHandler : MonoBehaviour
{

    [SerializeField] private BallEnabler ballEnabler;
    [SerializeField] private BallScript ballScript;
    [SerializeField] private ScoreArea scoreArea;

    void Update()
    {
        if (ballEnabler.playerWasHere && Multiplayer.Instance.GetUsers().Count == 1)
        {
            //Debug.Log("Reset jest");
            scoreArea.ResetScore();
            ballScript.StopBall();
            // ballScript.enabled = false;

            ballEnabler.enabled = true;
            ballEnabler.playerWasHere = false;
            ballEnabler.StartCheck();
        }
    }
}
