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
    [SerializeField] private AudioClip goalSound;
    private AudioSource audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BlueScore")) {
            redScore++;
            PlayGoalSound();
            StartCoroutine(BallCoroutine());
        }
        else if(other.CompareTag("RedScore")) {
            blueScore++;
            PlayGoalSound();
            StartCoroutine(BallCoroutine());
        } else return;

        scoreText.text = $"<color=blue>{blueScore}</color> : <color=red>{redScore}</color>";
    }

    private void PlayGoalSound()
    {
        if (goalSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(goalSound);
        }
    }


    private IEnumerator BallCoroutine() {
        gameObject.GetComponent<BallScript>().StopBall();
        yield return new WaitForSeconds(3);
        gameObject.GetComponent<BallScript>().StartBall();
    }
}
