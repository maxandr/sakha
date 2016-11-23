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
                if (transform.position.x < player.transform.position.x)
                {
                    Vector2 myVel = myBody.velocity;
                    myVel.x = -myTrans.right.x * -speed;
                    myBody.velocity = myVel;
                }
                else
                {
                    Vector2 myVel = myBody.velocity;
                    myVel.x = -myTrans.right.x * speed;
                    myBody.velocity = myVel;
                }
                SetAutoFlip();
            }
        }
    }
    private void SetAutoFlip()
    {
        if (myBody.velocity.x > 0)
        {
            Vector3 theScale = transform.localScale;
            if (theScale.x < 0)
            {
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }
        if (myBody.velocity.x < 0)
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