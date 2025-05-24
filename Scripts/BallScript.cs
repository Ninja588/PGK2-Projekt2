using Alteruna;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 30.0f;
    [SerializeField] private AudioSource audioSource;


    private RigidbodySynchronizable rbS;
    private Rigidbody rb;
    private Vector3 lastSpeed;


    void Start()
    {
        rbS = GetComponent<RigidbodySynchronizable>();
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(initialSpeed, 0, 1.0f);
        lastSpeed = rb.linearVelocity;
        // audioSource = GetComponent<AudioSource>();
        //Debug.Log("Start speed (Vec3): " + rb.linearVelocity);
        //Debug.Log("Start speed (float): " + rb.linearVelocity.magnitude);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rbS.velocity.magnitude >= 50f) return;
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Paddle"))
        {
            if(rb.linearVelocity.magnitude < lastSpeed.magnitude) {
                rb.linearVelocity = lastSpeed;
                //Debug.Log("Last speed (+): " + lastSpeed.magnitude);
            }
            else if(-rb.linearVelocity.magnitude < lastSpeed.magnitude) {
                rb.linearVelocity = -lastSpeed;
                //Debug.Log("Last speed (-): " + lastSpeed.magnitude);
            }
            Vector3 reflection = Vector3.Reflect(rb.linearVelocity, collision.contacts[0].normal);

            //Debug.Log("Collision speed (Vec3): " + rb.linearVelocity);
            //Debug.Log("Contacts normal: " + collision.contacts[0].normal);
            //Debug.Log("Reflection: " + reflection);

            float currentSpeed = rb.linearVelocity.magnitude;
            //Debug.Log("Previous speed: " + rb.linearVelocity);

            float speedMultiplier = 1.05f;
            float newSpeed = Mathf.Min(currentSpeed * speedMultiplier, 120.0f);

            //Debug.Log("New speed: " + newSpeed + " Previous speed: " + currentSpeed);

            rb.linearVelocity = reflection.normalized * newSpeed;
            lastSpeed = rb.linearVelocity;
            //Debug.Log("New speed: " + rb.linearVelocity);

            if (audioSource != null)
            {
                //Debug.Log("chuj");
                audioSource.Play();
            }

            rbS.ForceUpdate();
        }
    }
    public void StopBall() {
        rbS.velocity = Vector3.zero;
        gameObject.transform.position = new Vector3(20f,3.14f,0f);
        rbS.ForceUpdate();
    }
    public void StartBall() {
        rb.linearVelocity = new Vector3(initialSpeed, 0, 1.0f);
        lastSpeed = rb.linearVelocity;
    }
}
