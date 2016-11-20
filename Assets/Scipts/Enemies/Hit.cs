using UnityEngine;
using System.Collections;

public class Hit : MonoBehaviour {
    public void Hitted() {
        Destroy(gameObject);
    }
}
