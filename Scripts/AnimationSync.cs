using Alteruna;
using UnityEngine;

public class AnimationSync : AttributesSync
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    [SynchronizableMethod]
    void UpdateSpeed(float animationBlend) {
        animator.SetFloat("Speed", animationBlend);
        animator.SetFloat("MotionSpeed", 1.0f);
    }

    [SynchronizableMethod]
    void UpdateJump(bool value) {
        animator.SetBool("Jump", value);
    }

    [SynchronizableMethod]
    void UpdateGrounded(bool isGrounded) {
        animator.SetBool("Grounded", isGrounded);
    }

    [SynchronizableMethod]
    void UpdateFreeFall(bool value) {
        animator.SetBool("FreeFall", value);
    }
}
