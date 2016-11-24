using UnityEngine;
using System.Collections;

public class UnitActions : MonoBehaviour {
    private UnityStandardAssets._2D.UnitParams unit_params;
    public bool isShooter = true;
    public bool isPuncher = true;
    private float punch_timer;
    private float punch_dur_timer;
    private bool punching;
    private GameObject PunchObject;

    public GameObject bullet_instantiate;
    public GameObject bullet;
    public float bulletSpeed;

    private float shoot_timer;
    [HideInInspector]
    public bool canShoot;
    [HideInInspector]
    public bool canPunch;
    [HideInInspector]
    public GameObject enemy;
    // Use this for initialization
    void Start() {
        unit_params = GetComponent<UnityStandardAssets._2D.UnitParams>();
        punch_timer = unit_params.punch_cd;
        punch_dur_timer = 0.0f;
        shoot_timer = 0.0f;
        punching = false;
        PunchObject = transform.Find("Punch").gameObject;
        canShoot = true;
        canPunch = false;
        enemy = null;
    }
    public void SetNULL()
    {
        punching = false;
        punch_dur_timer = 0.0f;
        PunchObject.GetComponent<PolygonCollider2D>().enabled = false;
        PunchObject.GetComponent<SpriteRenderer>().enabled = false;
        shoot_timer = 0.0f;
    }
    // Update is called once per frame
    void FixedUpdate () {
        if (canPunch)
        {
            if (isPuncher)
            {
                if (punch_timer >= unit_params.punch_cd)
                {
                    Punch();
                    punch_timer = 0.0f;
                }
                else
                {
                    punch_timer += Time.deltaTime;
                }
            }
            if (punching)
            {
                punch_dur_timer += Time.deltaTime;
                if (punch_dur_timer >= unit_params.punch_duration)
                {
                    punching = false;
                    punch_dur_timer = 0.0f;
                    PunchObject.GetComponent<PolygonCollider2D>().enabled = false;
                    PunchObject.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
        if (canShoot && isShooter)
        {
            if (shoot_timer >= unit_params.punch_cd)
            {
                Shoot();
                shoot_timer = 0.0f;
            }
            else
            {
                shoot_timer += Time.deltaTime;
            }
        }
        

	}

    void Punch() {
        punching = true;
        PunchObject.GetComponent<PolygonCollider2D>().enabled = true;
        PunchObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    void Shoot()
    {
        if (enemy)
        {
            int direction = 1;
            int bullRot = 0;
            if (transform.position.x > enemy.transform.position.x)
            {
                direction = -1;
                bullRot = 180;
            }

            GameObject clone = Instantiate(bullet, bullet_instantiate.transform.position, bullet_instantiate.transform.rotation) as GameObject;
            clone.GetComponent<UnityStandardAssets._2D.BulletDestroy>().myCreatorTag = tag;
            clone.transform.Rotate(new Vector3(0, 0, bullRot));
            clone.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * direction, 0);
        }
    }
}
