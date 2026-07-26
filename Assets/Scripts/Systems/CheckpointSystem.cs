using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    [Header("Animation Clips")]
    public AnimationClip clipInactive;
    public AnimationClip clipActive;

    private Animator _anim;
    private bool _isActivated = false;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isActivated) return;
        if (!other.CompareTag("Player")) return;

        _isActivated = true;

        SwapClip(clipActive);

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
            player.SetRespawnPoint(transform);
    }

    void SwapClip(AnimationClip newClip)
    {
        AnimatorOverrideController overrideController =
            new AnimatorOverrideController(_anim.runtimeAnimatorController);

        overrideController[clipInactive] = newClip;

        _anim.runtimeAnimatorController = overrideController;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}