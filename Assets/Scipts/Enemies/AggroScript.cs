using UnityEngine;
using System.Collections;

public class AggroScript : MonoBehaviour {
    private bool isChased = false;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && transform.parent.GetComponent<isPlayerVisible>().IsPlayerVisible(other.gameObject) && !isChased)
        {
            isChased = true;
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
