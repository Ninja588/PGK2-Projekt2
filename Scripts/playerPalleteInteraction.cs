using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class playerPalleteInteraction : MonoBehaviour
{
    //public PlayerMovement playerMovement;
    //private RaycastHit raycastHit;
    public Alteruna.Avatar avatar;
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
        if(!avatar.IsMe) return;

        Rigidbody collidingObject = hit.collider.attachedRigidbody;

        if(collidingObject == null || collidingObject.isKinematic || !collidingObject.CompareTag("Paddle")) {
            return;
        }
        //Debug.Log(hit.controller.velocity.magnitude);
        Vector3 pushDirection = new Vector3(0,0,hit.moveDirection.z);
        float force = Mathf.Max(hit.controller.velocity.magnitude,1.0f);

        PaddleSync paddleSync = collidingObject.GetComponent<PaddleSync>();

        // Vector3 collisionPoint = hit.point;
        if(hit.normal.x > 0.0f) {
            return;
        }

        if(paddleSync!=null) {
            paddleSync.BroadcastRemoteMethod("Move",pushDirection,force);
        }
        //Debug.Log(hit.normal);
        //collidingObject.linearVelocity = pushDirection * Mathf.Max(hit.controller.velocity.magnitude,1.0f);
        //collidingObject.AddForceAtPosition(pushDirection * 4.0f,collisionPoint);
    }
}
