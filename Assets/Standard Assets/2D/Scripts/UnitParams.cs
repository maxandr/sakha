using UnityEngine;
using System.Collections;
namespace UnityStandardAssets._2D
{
    public class UnitParams : MonoBehaviour
    {
        public float speed;
        public float max_health;
        [HideInInspector]
        public float current_health;
        void Awake()
        {
            current_health = max_health;
        }
        public void Hitted()
        {
            current_health -= 1;
            if (current_health == 0)
            {
                if (gameObject.tag == "Player")
                {
                    Application.LoadLevel(Application.loadedLevel);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

}