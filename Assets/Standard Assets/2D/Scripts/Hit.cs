using UnityEngine;
using System.Collections;

public class Hit : MonoBehaviour {
    public void Hitted(int dmg) {
        gameObject.GetComponent<UnityStandardAssets._2D.UnitParams>().Hitted(dmg);
    }
}
