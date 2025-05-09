using System.Collections;
using System.Data.Common;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreArea : MonoBehaviour
{
    private Text scoreText;
    private int blueScore = 0;
    private int redScore = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BlueScore")) {
            redScore++;
            StartCoroutine(BallCoroutine());
        }
        else if(other.CompareTag("RedScore")) {
            blueScore++;
            StartCoroutine(BallCoroutine());
        } else return;
        scoreText.text = $"<color=blue>{blueScore}</color> : <color=red>{redScore}</color>";
    }

    private IEnumerator BallCoroutine() {
        gameObject.GetComponent<BallScript>().StopBall();
        yield return new WaitForSeconds(3);
        gameObject.GetComponent<BallScript>().StartBall();
    }
}
