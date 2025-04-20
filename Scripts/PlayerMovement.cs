using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Values")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    public Transform player;
    public Transform orientation;
    public CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;
 
    [SerializeField]
    private float cameraYOffset = 0.4f;
    private Camera playerCamera;

    private Slider slider;

    private Alteruna.Avatar _avatar;
    private RaycastHit raycastHit;
    private float punchForce = 15.0f;
 
    void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>();
 
        if (!_avatar.IsMe)
            return;

        characterController = GetComponent<CharacterController>();

        slider = GameObject.Find("PunchForceSlider").GetComponent<Slider>();
        
        playerCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
 
    void Update()
    {
        if (!_avatar.IsMe)
            return;
        
        bool isRunning = false;
 
        isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
 
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
 
        if(Input.GetMouseButton(0)) {
            punchForce += 0.1f;
            slider.value = punchForce;
        }

        if(Input.GetMouseButtonUp(0)) {
            Ray ray = new(player.position, player.forward);
            Physics.Raycast(ray,out raycastHit,5.0f);
            Rigidbody collidingObject = raycastHit.rigidbody;
           
            if(collidingObject == null || collidingObject.isKinematic || !collidingObject.CompareTag("Paddle")) {
                punchForce = 15.0f;
                return;
            }
            
            Debug.Log(punchForce);
            
            Vector3 pushDirection = new(0,0,raycastHit.normal.z * -1.0f);
            float clampedForce = Mathf.Min(punchForce, 70f);

            PaddleSync paddleSync = collidingObject.GetComponent<PaddleSync>();

            if(paddleSync != null) {
                paddleSync.BroadcastRemoteMethod("Push", pushDirection, clampedForce);
            }
            
            //collidingObject.linearVelocity = pushDirection.normalized * Mathf.Min((int)punchForce, 70.0f);
            
            punchForce = 15.0f;
        }

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
 
        characterController.Move(moveDirection * Time.deltaTime);
 
        if (canMove && playerCamera != null)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

            player.transform.forward = orientation.forward;
        }
    }
}
