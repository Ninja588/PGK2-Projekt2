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
            scoreArea.ResetScore();
            ballScript.StopBall();

            ballEnabler.enabled = true;
            ballEnabler.playerWasHere = false;
            ballEnabler.StartCheck();
        }
    }
}
