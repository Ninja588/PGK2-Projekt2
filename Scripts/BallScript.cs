using System.Collections;
using Alteruna;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 30.0f;
    [SerializeField] private AudioSource audioSource;


    private RigidbodySynchronizable rbS;
    private Rigidbody rb;
    private Vector3 lastSpeed;
    private BallAbilitiesSync abilitesSync;


    void Start()
    {
        rbS = GetComponent<RigidbodySynchronizable>();
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(initialSpeed, 0, 1.0f);
        lastSpeed = rb.linearVelocity;
        abilitesSync = GetComponent<BallAbilitiesSync>();

    }

    void OnCollisionEnter(Collision collision)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        if (!Multiplayer.Instance.GetUser().IsHost) return;
        if (rbS.velocity.magnitude >= 50f) return;
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Paddle"))
        {

            ContactPoint contact = collision.contacts[0];

            Vector3 currentVelocity = rb.linearVelocity;
            Vector3 reflectedVelocity = Vector3.Reflect(currentVelocity, contact.normal);

            float speedMultiplier = 1.05f;
            float newSpeed = Mathf.Min(currentVelocity.magnitude * speedMultiplier, 120.0f);

            rb.linearVelocity = reflectedVelocity.normalized * newSpeed;
            lastSpeed = rb.linearVelocity;


            rbS.ForceUpdate();
        }
    }

    public void StopBall()
    {
        rbS.velocity = Vector3.zero;
        gameObject.transform.position = new Vector3(20f, 3.14f, 0f);
        rbS.ForceUpdate();
        abilitesSync.StopAllCoroutines();
    }
    public void StartBall()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(initialSpeed, 0, 1.0f);
        lastSpeed = rb.linearVelocity;
    }
}
