using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioClip landClip;
    [SerializeField] private AudioSource audioSource;

    public void OnFootstep()
    {
        if (footstepClips.Length == 0) return;
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.PlayOneShot(clip);
    }

    public void OnLand()
    {
        if (landClip == null) return;
        audioSource.PlayOneShot(landClip);
    }
}
