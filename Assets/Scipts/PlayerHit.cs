using UnityEngine;
using System.Collections;

public class PlayerHit : MonoBehaviour {
    private UnityStandardAssets._2D.PlatformerCharacter2D player;
    void Awake() {
        player = transform.parent.gameObject.GetComponent<UnityStandardAssets._2D.PlatformerCharacter2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "EnemyHitCollider" && !player.teleporting)
        {
           // gameObject.transform.parent.GetComponent<Hit>().Hitted();
        }
        if (other.gameObject.layer == 20 && player.teleporting)
        {
            player.StopTeleport();
        }
    }
}
