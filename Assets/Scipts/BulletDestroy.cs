using UnityEngine;
using System.Collections;

public class BulletDestroy : MonoBehaviour {
    public float death_time;
	// Update is called once per frame
	void Update () {
        death_time -= Time.deltaTime;
        if(death_time<=0.0f)
        {
            Destroy(gameObject);
        }
    }
}
