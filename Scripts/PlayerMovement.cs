using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : AttributesSync
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

    private Animator animator;
    private float animationBlend;
    private AnimationSync animationSync;
    private float lastSentSpeed = 0;
    private bool lastIsFalling = false;
    private bool lastIsGrounded = true;

    void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>();

        if (!_avatar.IsMe)
            return;

        characterController = GetComponent<CharacterController>();

        slider = GameObject.Find("PunchForceSlider").GetComponent<Slider>();
        animator = GetComponentInChildren<Animator>();
        animationSync = GetComponentInChildren<AnimationSync>();

        playerCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        foreach (var rend in GetComponentsInChildren<SkinnedMeshRenderer>())
            rend.enabled = false;

        animator.SetBool("Grounded", true);
        animator.SetBool("Jump", false);
        animator.SetBool("FreeFall", false);
        animationBlend = 0f;
    }

    void Update()
    {
        if (!_avatar.IsMe) return;

        bool wasGrounded = characterController.isGrounded;

        bool isRunning = false;
        isRunning = Input.GetKey(KeyCode.LeftShift);
        float inputZ = Input.GetAxis("Vertical");
        float inputX = Input.GetAxis("Horizontal");
        Vector2 inputVec = new Vector2(inputZ, inputX);
        float inputMagnitude = Mathf.Clamp01(inputVec.magnitude);

        float targetSpeed = isRunning ? runningSpeed : walkingSpeed;
        if (inputMagnitude == 0f) targetSpeed = 0f;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float velY = moveDirection.y;
        moveDirection = (forward * inputZ + right * inputX).normalized * targetSpeed;
        moveDirection.y = velY;

        if (Input.GetButtonDown("Jump") && characterController.isGrounded && canMove)
        {
            moveDirection.y = jumpSpeed;
            animationSync.BroadcastRemoteMethod("UpdateJump", true);
            animationSync.BroadcastRemoteMethod("UpdateGrounded", false);
            //animator.SetBool("Jump", true);
            //animator.SetBool("Grounded", false);
        }

        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);

        float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;
        float normalizedSpeed = Mathf.Clamp01(currentHorizontalSpeed / runningSpeed);

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * 10f);
        //animationSync.BroadcastRemoteMethod("UpdateSpeed", animationBlend);
        // animator.SetFloat("Speed", animationBlend);
        // animator.SetFloat("MotionSpeed", 1.0f);
        if (Mathf.Abs(animationBlend - lastSentSpeed) > 2.0f)
        {
            //animator.SetFloat("Speed", animationBlend);
            //animator.SetFloat("MotionSpeed", 1.0f);
            lastSentSpeed = animationBlend;
            animationSync.BroadcastRemoteMethod("UpdateSpeed", animationBlend);
        }
        //Debug.Log("animationBlend: " + animationBlend + " MotionSpeed: " + inputMagnitude);

        bool isGrounded = characterController.isGrounded;
        //animator.SetBool("Grounded", isGrounded);
        if (isGrounded != lastIsGrounded)
        {
            //animator.SetBool("Grounded", isGrounded);
            animationSync.BroadcastRemoteMethod("UpdateGrounded", isGrounded);
            lastIsGrounded = isGrounded;
        }

        if (!wasGrounded && isGrounded)
        {
            animationSync.BroadcastRemoteMethod("UpdateJump", false);
            //animator.SetBool("Jump", false);
            animationSync.BroadcastRemoteMethod("UpdateFreeFall", false);
            //animator.SetBool("FreeFall", false);
        }

        bool isFalling = !isGrounded && moveDirection.y < 0;
        //animator.SetBool("FreeFall", isFalling);
        if (isFalling != lastIsFalling)
        {
            //animator.SetBool("FreeFall", isFalling);
            animationSync.BroadcastRemoteMethod("UpdateFreeFall", isFalling);
            lastIsFalling = isFalling;
        }


        if (Input.GetMouseButton(0))
        {
            punchForce += 0.1f;
            slider.value = punchForce;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = new Ray(player.position, player.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                Rigidbody rb = hit.rigidbody;
                if (rb != null && !rb.isKinematic && rb.CompareTag("Paddle"))
                {
                    Vector3 pushDir = -hit.normal;
                    float clamped = Mathf.Min(punchForce, 70f);
                    var sync = rb.GetComponent<PaddleSync>();
                    if (sync != null) sync.BroadcastRemoteMethod("Push", pushDir, clamped);
                }
            }
            punchForce = 15f;
            slider.value = punchForce;
        }

        if (canMove && playerCamera != null)
        {
            rotationX = Mathf.Clamp(rotationX - Input.GetAxis("Mouse Y") * lookSpeed, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            player.transform.forward = orientation.forward;
        }
    }
    
    public void ApplyPush(Vector3 direction, float strength)
    {
        transform.position += direction.normalized * strength * Time.deltaTime;
    }
}
