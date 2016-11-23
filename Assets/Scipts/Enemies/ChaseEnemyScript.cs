using UnityEngine;
using System.Collections;

public class ChaseEnemyScript : MonoBehaviour
{

    private float speed = 1;
    Rigidbody2D myBody;
    Transform myTrans;
    float myWidth, myHeight;
    private bool going_right;
    private Animator m_Anim;
    [HideInInspector]
    public GameObject player;

    private LayerMask platformMask;

    private float hitCollider_w;
    private float enemy_hitCollider_w;
    static Vector2 toVector2(Vector3 vect)
    {
        Vector2 tRetVec = new Vector2(vect.x, vect.y);
        return tRetVec;
    }
    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
        platformMask = 1 << 20;
    }
    public void ChaseEnemy(GameObject enemy)
    {
        player = enemy;
        GameObject tEnemyHitCollider = player.transform.FindChild("EnemyHitCollider").gameObject;
        enemy_hitCollider_w = tEnemyHitCollider.GetComponent<BoxCollider2D>().size.x * tEnemyHitCollider.transform.localScale.x * player.transform.localScale.x;
    }

    void Start()
    {
        speed = gameObject.GetComponent<UnityStandardAssets._2D.UnitParams>().speed;
        player = null;
        going_right = true;
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        myWidth = GetComponent<BoxCollider2D>().size.x * gameObject.transform.localScale.x;
        myHeight = GetComponent<BoxCollider2D>().size.y * gameObject.transform.localScale.y;

        GameObject tHitCollider = transform.FindChild("EnemyHitCollider").gameObject;
        hitCollider_w = tHitCollider.GetComponent<BoxCollider2D>().size.x * tHitCollider.transform.localScale.x * gameObject.transform.localScale.x;
    }
    void FixedUpdate()
    {
        if (player)
        {
            bool isGrounded = false;
            Vector2 lineCastPos;
            if (going_right)
            {
                lineCastPos = toVector2(myTrans.position) + toVector2(myTrans.right) * myWidth/2 + Vector2.down * myHeight * 0.8f;
                Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
                Debug.DrawLine(lineCastPos, lineCastPos - toVector2(myTrans.right) * .05f);
            }
            else
            {
                lineCastPos = toVector2(myTrans.position) - toVector2(myTrans.right) * myWidth/2 + Vector2.down * myHeight * 0.8f;
                Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
                Debug.DrawLine(lineCastPos, lineCastPos + toVector2(myTrans.right) * .05f);
            }
            isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down * myHeight, platformMask);

            m_Anim.SetBool("Ground", isGrounded);
            m_Anim.SetFloat("vSpeed", myBody.velocity.y);
            m_Anim.SetFloat("Speed", Mathf.Abs(myBody.velocity.x));

            if (player)
            {
                if ((transform.position.x + hitCollider_w / 2) < (player.transform.position.x - enemy_hitCollider_w / 2))
                {
                    Vector2 myVel = myBody.velocity;
                    myVel.x = -myTrans.right.x * -speed;
                    myBody.velocity = myVel;
                    if (GetComponent<UnitActions>().enabled)
                    {
                        GetComponent<UnitActions>().SetNULL();
                        GetComponent<UnitActions>().enabled = false;
                    }
                }
                else if((player.transform.position.x + enemy_hitCollider_w / 2) < (transform.position.x - hitCollider_w / 2))
                {
                    Vector2 myVel = myBody.velocity;
                    myVel.x = -myTrans.right.x * speed;
                    myBody.velocity = myVel;
                    if (GetComponent<UnitActions>().enabled)
                    {
                        GetComponent<UnitActions>().SetNULL();
                        GetComponent<UnitActions>().enabled = false;
                    }
                }
                else
                {
                    GetComponent<UnitActions>().enabled = true;
                }
                SetAutoFlip();
            }
        }
    }
    private void SetAutoFlip()
    {
        if (transform.position.x < player.transform.position.x)
        {
            Vector3 theScale = transform.localScale;
            if (theScale.x < 0)
            {
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }
        else 
        {
            Vector3 theScale = transform.localScale;
            if (theScale.x > 0)
            {
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }
    }

}