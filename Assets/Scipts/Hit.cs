using UnityEngine;
using System.Collections;

public class Hit : MonoBehaviour {
    public void Hitted() {
        gameObject.GetComponent<UnityStandardAssets._2D.UnitParams>().Hitted();
    }
}
