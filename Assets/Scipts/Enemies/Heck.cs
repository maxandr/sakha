using UnityEngine;
using System.Collections;

public class Heck : MonoBehaviour
{
    public LayerMask enemyMask;
    public LayerMask wallMask;
    public float speed = 1;
    Rigidbody2D myBody;
    Transform myTrans;
    float myWidth, myHeight;
    private bool going_right;
    private Animator m_Anim;     
    public GameObject[] walls;
    Vector2 toVector2(Vector3 vect)
    {
        Vector2 tRetVec = new Vector2(vect.x, vect.y);
        return tRetVec;
    }
    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
    }
    void Start()
    {
        going_right = true;
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        SpriteRenderer mySprite = this.GetComponent<SpriteRenderer>();
        myWidth = GetComponent<BoxCollider2D>().size.x;
        myHeight = GetComponent<BoxCollider2D>().size.y; 
        foreach (GameObject wall in walls)
        {
            GameObject clone = Instantiate(wall) as GameObject;
            clone.transform.position = wall.transform.position;
            Vector2 tSize = wall.GetComponent<BoxCollider2D>().size;
            tSize.x = tSize.x * gameObject.transform.localScale.x;
            tSize.y = tSize.y * gameObject.transform.localScale.y;
            clone.GetComponent<BoxCollider2D>().size = tSize;
            clone.layer = 23;
            Destroy(wall);
        }

    }

    void FixedUpdate()
    {
        bool isGrounded = false;
        bool isBlocked = false; 
        if (going_right)
        {
            Vector2 lineCastPos = toVector2(myTrans.position) + toVector2(myTrans.right) * myWidth + Vector2.down * myHeight * 0.8f;
            Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
            isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
            Debug.DrawLine(lineCastPos, lineCastPos - toVector2(myTrans.right) * .05f);
            isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - toVector2(myTrans.right) * .05f, wallMask);
        }
        else
        {
            Vector2 lineCastPos = toVector2(myTrans.position) - toVector2(myTrans.right) * myWidth + Vector2.down * myHeight * 0.8f;
            Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
            isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
            Debug.DrawLine(lineCastPos, lineCastPos + toVector2(myTrans.right) * .05f);
            isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - toVector2(myTrans.right) * .05f, wallMask);
        }
        m_Anim.SetBool("Ground", isGrounded);
        m_Anim.SetFloat("vSpeed", myBody.velocity.y);
        m_Anim.SetFloat("Speed", Mathf.Abs(myBody.velocity.x));
        if (!isGrounded || isBlocked)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            going_right = !going_right;
        }

        Vector2 myVel = myBody.velocity;
        if(going_right)
        {
            myVel.x = -myTrans.right.x * -speed;
        }
        else
        {
            myVel.x = -myTrans.right.x * speed;
        }
        myBody.velocity = myVel;
    }
}