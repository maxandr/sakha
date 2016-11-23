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
    // Use this for initialization
    void Start() {
        unit_params = GetComponent<UnityStandardAssets._2D.UnitParams>();
        punch_timer = unit_params.punch_cd;
        punch_dur_timer = 0.0f;
        punching = false;
        PunchObject = transform.Find("Punch").gameObject;
    }
    public void SetNULL()
    {
        punching = false;
        punch_dur_timer = 0.0f;
        PunchObject.GetComponent<PolygonCollider2D>().enabled = false;
        PunchObject.GetComponent<SpriteRenderer>().enabled = false;
    }
    // Update is called once per frame
    void FixedUpdate () {
        if (isPuncher) {
            if (punch_timer >= unit_params.punch_cd) {
                Punch();
                punch_timer = 0.0f;
            }
            else
            {
                punch_timer += Time.deltaTime;
            }
        }
        if (punching) {
            punch_dur_timer += Time.deltaTime;
            if (punch_dur_timer >= unit_params.punch_duration) {
                punching = false;
                punch_dur_timer = 0.0f;
                PunchObject.GetComponent<PolygonCollider2D>().enabled = false;
                PunchObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

	}

    void Punch() {
        punching = true;
        PunchObject.GetComponent<PolygonCollider2D>().enabled = true;
        PunchObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
