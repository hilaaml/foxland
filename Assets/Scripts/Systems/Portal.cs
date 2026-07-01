using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    private Animator _anim;
    private bool _playerEntering = false;

    [Header("Enter Effect")]
    public float enterEffectDuration = 0.6f;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void Activate()
    {
        _anim.SetTrigger("Open");
        gameObject.SetActive(true);
    }

    public void OnCloseAnimationFinished()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_playerEntering) return;
        if (!other.CompareTag("Player")) return;

        _playerEntering = true;
        StartCoroutine(PlayerEnterPortalRoutine(other.gameObject));
    }

    IEnumerator PlayerEnterPortalRoutine(GameObject player)
    {
        // matikan kontrol player saat efek main
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        Vector3 startScale = player.transform.localScale;
        Vector3 startPos = player.transform.position;
        Vector3 targetPos = transform.position; // tertarik ke tengah portal

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        Color startColor = sr != null ? sr.color : Color.white;

        float t = 0f;
        while (t < enterEffectDuration)
        {
            t += Time.deltaTime;
            float progress = t / enterEffectDuration;

            // tertarik ke tengah portal + mengecil + transparan
            player.transform.position = Vector3.Lerp(startPos, targetPos, progress);
            player.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);

            if (sr != null)
            {
                Color c = startColor;
                c.a = Mathf.Lerp(1f, 0f, progress);
                sr.color = c;
            }

            yield return null;
        }

        // baru pindah ke level berikutnya setelah efek selesai
        LevelManager.Instance?.OnPlayerReachPortal();
    }
}