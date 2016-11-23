using UnityEngine;
using System.Collections;
namespace UnityStandardAssets._2D
{
    public class UnitParams : MonoBehaviour
    {
        public float speed;
        public float max_health;
        public float punch_cd;
        public float punch_duration;
        public int punch_dmg;
        [HideInInspector]
        public float current_health;
        void Awake()
        {
            current_health = max_health;
        }
        public void Hitted(int dmg)
        {
            current_health -= dmg;
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