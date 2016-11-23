using UnityEngine;
using System.Collections;

public class AggroScript : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(transform.parent.gameObject.GetComponent<Patrol>())
            {
                transform.parent.gameObject.GetComponent<Patrol>().enabled = false;
            }
            if (transform.parent.gameObject.GetComponent<ChaseEnemyScript>())
            {
                transform.parent.gameObject.GetComponent<ChaseEnemyScript>().ChaseEnemy(other.gameObject);
            }

            //Destroy(transform.parent.gameObject);
        }
    }
}
