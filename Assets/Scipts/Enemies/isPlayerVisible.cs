using UnityEngine;
using System.Collections;

public class isPlayerVisible : MonoBehaviour {
    private GameObject aggro_circle;
    void Awake() {
        aggro_circle = transform.FindChild("AggroCircle").gameObject;
    }

    public bool IsPlayerVisible(GameObject player)
    {
        if (player && aggro_circle)
        {
            float maxRange = aggro_circle.GetComponent<CircleCollider2D>().radius * Mathf.Max(transform.localScale.x, transform.localScale.y) * Mathf.Max(aggro_circle.transform.localScale.x , aggro_circle.transform.localScale.y);
            if (Vector3.Distance(transform.position, player.transform.position) < maxRange)
            {
                RaycastHit2D tRay = Physics2D.Raycast(transform.position, player.transform.position - transform.position, maxRange, 1 << 21 | 1<<20);
                if (tRay.transform.gameObject == player)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
