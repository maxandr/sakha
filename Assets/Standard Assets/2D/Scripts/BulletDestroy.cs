using UnityEngine;
using System.Collections;
namespace UnityStandardAssets._2D
{
    public class BulletDestroy : MonoBehaviour
    {
        [HideInInspector]
        public string myCreatorTag;
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
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
            {
                if (other.gameObject.tag != myCreatorTag)
                {
                    //if(other.gameObject.tag == "Player" && other.gameObject.GetComponent<>
                    if (other.gameObject.GetComponent<Hit>().Hitted(1))
                    {
                        Destroy(gameObject);
                    }
                }
                if (other.gameObject.layer == 20)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
