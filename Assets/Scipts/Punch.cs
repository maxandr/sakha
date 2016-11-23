using UnityEngine;
using System.Collections;

public class Punch : MonoBehaviour {
    private UnityStandardAssets._2D.UnitParams unit_params;
    void Start()
    {
        unit_params = transform.parent.GetComponent<UnityStandardAssets._2D.UnitParams>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "EnemyHitCollider")
        {
            other.gameObject.transform.parent.GetComponent<Hit>().Hitted(unit_params.punch_dmg);
        }
    }
}
