using UnityEngine;

public class PatrolSFXManager : MonoBehaviour
{
    private PatrolEnemy[] _enemies;
    private Transform _finn;

    void Start()
    {
        _enemies = FindObjectsByType<PatrolEnemy>(FindObjectsSortMode.None);

        var player = FindFirstObjectByType<PlayerController>();
        if (player != null) _finn = player.transform;
    }

    void Update()
    {
        if (_finn == null || AudioManager.Instance == null) return;

        float radius = AudioManager.Instance.GetPatrolHearRadius();
        bool anyNearby = false;

        foreach (var enemy in _enemies)
        {
            if (enemy == null) continue;
            if (Vector2.Distance(_finn.position, enemy.transform.position) <= radius)
            {
                anyNearby = true;
                break;
            }
        }

        AudioManager.Instance.UpdatePatrolProximity(anyNearby);
    }
}