using UnityEngine;
using System.Collections;

public class BulletDestroy : MonoBehaviour
{
    public float death_time;
    // Update is called once per frame
    void Update()
    {
        death_time -= Time.deltaTime;
        if (death_time <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Hit>().Hitted(1);
            Destroy(gameObject);
        }
        if (other.gameObject.layer == 20) { 
            Destroy(gameObject);
        }
    }
}
