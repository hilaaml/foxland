using UnityEngine;

// Base class untuk item yang mengambang (bobbing) dan diambil pemain lewat trigger.
// Gem & Cherry cukup override OnCollected() untuk perilaku spesifiknya masing-masing.
public abstract class CollectibleBase : MonoBehaviour
{
    [Header("Animation")]
    public float bobSpeed = 2f;
    public float bobHeight = 0.2f;

    private Vector3 _startPos;

    protected virtual void Start()
    {
        _startPos = transform.position;
    }

    protected virtual void Update()
    {
        float newY = _startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(_startPos.x, newY, _startPos.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        OnCollected(other);
        Destroy(gameObject);
    }

    protected abstract void OnCollected(Collider2D player);
}