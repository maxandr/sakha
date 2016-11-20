using UnityEngine;
using System.Collections;

public class PlayerHit : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "EnemyHitCollider")
        {
            gameObject.transform.parent.GetComponent<Hit>().Hitted();
        }
    }
}
