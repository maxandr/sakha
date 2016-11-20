using UnityEngine;
using System.Collections;

public class AggroScript : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.gameObject.GetComponent<Heck>().player = other.gameObject;
            //Destroy(transform.parent.gameObject);
        }
    }
}
