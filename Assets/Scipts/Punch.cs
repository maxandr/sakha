using UnityEngine;
using System.Collections;

public class Punch : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "EnemyHitCollider")
        {
            other.gameObject.transform.parent.GetComponent<Hit>().Hitted();
        }
    }
}
