using UnityEngine;

public class Cherry : CollectibleBase
{
    protected override void OnCollected(Collider2D player)
    {
        player.GetComponent<PlayerEnergy>()?.ConsumeCherry();
    }
}