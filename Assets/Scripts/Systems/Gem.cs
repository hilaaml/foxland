using UnityEngine;

public class Gem : MonoBehaviour
{
    [Header("Animation")]
    public float bobSpeed = 2f;
    public float bobHeight = 0.2f;

    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        // efek mengambang naik-turun
        float newY = _startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(_startPos.x, newY, _startPos.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        AudioManager.Instance?.PlaySFX("CollectGem");
        GameManager.Instance?.CollectGem();
        Destroy(gameObject);
    }
}