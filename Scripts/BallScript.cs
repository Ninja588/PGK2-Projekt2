using Alteruna;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10f;

    private RigidbodySynchronizable rbS;

    void Start()
    {
        rbS = GetComponent<RigidbodySynchronizable>();
        rbS.velocity = new Vector3(initialSpeed, 0, initialSpeed);
    }

    void Update()
    {
        //Debug.Log(rbS.velocity.magnitude);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rbS.velocity.magnitude >= 50f) return;
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Paddle"))
        {
            Vector3 reflection = Vector3.Reflect(rbS.velocity, collision.contacts[0].normal);

            float currentSpeed = rbS.velocity.magnitude;
            float speedMultiplier = 1.05f;
            float newSpeed = Mathf.Max(currentSpeed * speedMultiplier, currentSpeed);

            rbS.velocity = reflection.normalized * newSpeed;
        }
    }
}
