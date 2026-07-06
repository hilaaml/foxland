using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class ParallaxLayer : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Tooltip("0 = diam total (jauh sekali), 1 = ikut gerak penuh dengan kamera")]
    [Range(0f, 1f)]
    public float parallaxFactor = 0.3f;

    [Tooltip("Jika true, layer juga parallax secara vertikal")]
    public bool parallaxVertical = false;

    [Header("Infinite Tiling")]
    [Tooltip("Source sprite yang akan di-clone otomatis. Drag child pertama di sini, atau biarkan kosong untuk auto-detect dari child pertama.")]
    public Transform sourceTile;

    [Tooltip("Jumlah tile extra di tiap sisi sebagai buffer (mencegah celah saat kamera gerak cepat). 1 = aman untuk kebanyakan kasus.")]
    [Range(1, 3)]
    public int bufferTiles = 1;

    private Camera _cam;
    private Vector3 _lastCamPos;
    private float _textureWidth;
    private List<Transform> _tiles = new List<Transform>();
    private int _tilesNeeded;

    void Start()
    {
        if (!Application.isPlaying) return;

        _cam = Camera.main;
        _lastCamPos = _cam.transform.position;

        SetupTiles();
    }

    void SetupTiles()
    {
        // tentukan source tile: pakai yang di-assign, atau child pertama
        if (sourceTile == null)
        {
            if (transform.childCount == 0)
            {
                Debug.LogError($"[ParallaxLayer] '{name}' butuh minimal 1 child sprite sebagai source tile.");
                return;
            }
            sourceTile = transform.GetChild(0);
        }

        SpriteRenderer sourceSR = sourceTile.GetComponent<SpriteRenderer>();
        if (sourceSR == null)
        {
            Debug.LogError($"[ParallaxLayer] Source tile '{sourceTile.name}' tidak punya SpriteRenderer.");
            return;
        }

        _textureWidth = sourceSR.bounds.size.x;
        if (_textureWidth <= 0f)
        {
            Debug.LogError($"[ParallaxLayer] Lebar sprite '{sourceTile.name}' = 0. Cek sprite sudah ter-assign dengan benar.");
            return;
        }

        // hitung berapa banyak tile dibutuhkan untuk cover lebar kamera + buffer
        float camViewWidth = GetCameraViewWidth();
        _tilesNeeded = Mathf.CeilToInt(camViewWidth / _textureWidth) + 1 + (bufferTiles * 2);

        // hapus semua child lain selain source (kalau ada sisa duplikasi manual lama)
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child != sourceTile)
            {
                if (Application.isPlaying) Destroy(child.gameObject);
                else DestroyImmediate(child.gameObject);
            }
        }

        // generate tile sebanyak _tilesNeeded, clone dari source
        _tiles.Clear();
        _tiles.Add(sourceTile);

        for (int i = 1; i < _tilesNeeded; i++)
        {
            Transform clone = Instantiate(sourceTile, transform);
            clone.name = $"{sourceTile.name}_clone{i}";
            _tiles.Add(clone);
        }

        // susun semua tile berjejer rapi, center di posisi kamera saat ini
        float startX = _cam.transform.position.x - (_tilesNeeded * _textureWidth / 2f);
        for (int i = 0; i < _tiles.Count; i++)
        {
            Vector3 pos = _tiles[i].position;
            pos.x = startX + (i * _textureWidth);
            _tiles[i].position = pos;
        }
    }

    float GetCameraViewWidth()
    {
        if (_cam == null) return 20f;
        float height = _cam.orthographicSize * 2f;
        float width = height * _cam.aspect;
        return width;
    }

    void LateUpdate()
    {
        if (!Application.isPlaying || _cam == null || _tiles.Count == 0) return;

        Vector3 delta = _cam.transform.position - _lastCamPos;

        Vector3 move = new Vector3(
            delta.x * parallaxFactor,
            parallaxVertical ? delta.y * parallaxFactor : 0f,
            0f);

        transform.position += move;
        _lastCamPos = _cam.transform.position;

        HandleInfiniteLoop();
    }

    void HandleInfiniteLoop()
    {
        float camX = _cam.transform.position.x;

        foreach (var tile in _tiles)
        {
            float distFromCam = camX - tile.position.x;

            // tile sudah lewat di belakang kamera -> lempar ke paling depan
            if (distFromCam > _textureWidth * (_tilesNeeded / 2f))
            {
                RepositionTile(tile, true);
            }
            // tile terlalu jauh di depan kamera (kamera gerak mundur) -> lempar ke belakang
            else if (distFromCam < -_textureWidth * (_tilesNeeded / 2f))
            {
                RepositionTile(tile, false);
            }
        }
    }

    void RepositionTile(Transform tile, bool moveForward)
    {
        float targetX = moveForward ? Mathf.NegativeInfinity : Mathf.Infinity;

        foreach (var t in _tiles)
        {
            if (t == tile) continue;
            if (moveForward && t.position.x > targetX) targetX = t.position.x;
            if (!moveForward && t.position.x < targetX) targetX = t.position.x;
        }

        Vector3 pos = tile.position;
        pos.x = moveForward ? targetX + _textureWidth : targetX - _textureWidth;
        tile.position = pos;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(2f, 4f, 0f));
    }
#endif
}