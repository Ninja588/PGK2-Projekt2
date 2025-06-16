using System.Collections;
using System.Data.Common;
using System.Linq;
using Alteruna;
using UnityEngine;
using UnityEngine.UI;

public class ScoreArea : AttributesSync
{
    [SerializeField] private Text scoreText;
    [SynchronizableField] private int blueScore = 0;
    [SynchronizableField] private int redScore = 0;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!Multiplayer.Instance.GetUser().IsHost) return;
        
        if (other.CompareTag("BlueScore"))
        {
            BroadcastRemoteMethod("RedScoreInc");
            PlayGoalSound();
            StartCoroutine(BallCoroutine());
        }
        else if (other.CompareTag("RedScore"))
        {
            BroadcastRemoteMethod("BlueScoreInc");
            PlayGoalSound();
            StartCoroutine(BallCoroutine());
        }

        BroadcastRemoteMethod("SetScoreText");
    }

    private void PlayGoalSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void ResetScore()
    {
        StopAllCoroutines();

        blueScore = 0;
        redScore = 0;

        scoreText.text = "";
    }

    private IEnumerator BallCoroutine()
    {
        gameObject.GetComponent<BallScript>().StopBall();
        yield return new WaitForSeconds(3);
        gameObject.GetComponent<BallScript>().StartBall();
    }

    [SynchronizableMethod]
    private void RedScoreInc()
    {
        redScore++;
    }

    [SynchronizableMethod]
    private void BlueScoreInc()
    {
        blueScore++;
    }

    [SynchronizableMethod]
    private void SetScoreText()
    {
        scoreText.text = $"<color=blue>{blueScore}</color> : <color=red>{redScore}</color>";
    }
}
