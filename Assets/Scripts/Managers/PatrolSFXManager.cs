using UnityEngine;

public class PatrolSFXManager : Singleton<PatrolSFXManager>
{
    [Header("Enemy Detection")]
    [Tooltip("Set ke Layer 'Enemy' yang dipakai semua PatrolEnemy.")]
    public LayerMask enemyLayerMask;

    private Camera _cam;

    void Update()
    {
        if (AudioManager.Instance == null) return;

        if (GameManager.Instance != null && GameManager.Instance.currentState != GameState.Playing)
            return;

        if (_cam == null)
        {
            _cam = Camera.main;
            if (_cam == null) return;
        }

        bool anyInFrame = IsEnemyInCameraView();
        AudioManager.Instance.UpdatePatrolProximity(anyInFrame);
    }

    bool IsEnemyInCameraView()
    {
        if (!_cam.orthographic) return false;

        float height = _cam.orthographicSize * 2f;
        float width = height * _cam.aspect;

        Vector2 camPos = _cam.transform.position;
        Vector2 bottomLeft = new Vector2(camPos.x - width / 2f, camPos.y - height / 2f);
        Vector2 topRight = new Vector2(camPos.x + width / 2f, camPos.y + height / 2f);

        Collider2D hit = Physics2D.OverlapArea(bottomLeft, topRight, enemyLayerMask);
        return hit != null;
    }
}