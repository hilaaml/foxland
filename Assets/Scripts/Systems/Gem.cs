using UnityEngine;

public class Gem : CollectibleBase
{
    protected override void OnCollected(Collider2D player)
    {
        AudioManager.Instance?.PlaySFX(SFXId.CollectGem);
        GameManager.Instance?.CollectGem();
    }
}