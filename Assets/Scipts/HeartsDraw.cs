using UnityEngine;
using System.Collections;

public class HeartsDraw : MonoBehaviour {
    public GameObject player;
    public GameObject[] hearts;
    void Update() {
        //todo govno
        foreach (GameObject heart in hearts)
        {
            foreach (Transform child in heart.transform)
            {
                if (child.name == "heart") {
                    child.gameObject.SetActive(false);
                }

            }
        }

        for (int i = 0; i < player.GetComponent<UnityStandardAssets._2D.UnitParams>().current_health; ++i)
        {
            foreach (Transform child in hearts[i].transform)
            {
                if (child.name == "heart")
                {
                    child.gameObject.SetActive(true);
                }

            }
        }
    }
}
