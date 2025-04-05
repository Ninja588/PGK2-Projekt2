using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class playerPalleteInteraction : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private RaycastHit raycastHit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody collidingObject = hit.collider.attachedRigidbody;
        if(collidingObject == null || collidingObject.isKinematic || !collidingObject.CompareTag("Paddle")) {
            return;
        }
        //Debug.Log(hit.controller.velocity.magnitude);
        Vector3 pushDirection = new Vector3(0,0,hit.moveDirection.z);
        // Vector3 collisionPoint = hit.point;
        if(hit.normal.x > 0.0f) {
            return;
        }
        Debug.Log(hit.normal);
        collidingObject.linearVelocity = pushDirection * Mathf.Max(hit.controller.velocity.magnitude,1.0f);
        //collidingObject.AddForceAtPosition(pushDirection * 4.0f,collisionPoint);
    }
}
