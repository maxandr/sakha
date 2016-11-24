using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour
{
   
    private float speed = 1;
    Rigidbody2D myBody;
    Transform myTrans;
    float myWidth, myHeight;
    private bool going_right;
    private Animator m_Anim;
    private GameObject patrolWalls;
    public ArrayList created_walls;
    [HideInInspector]
    public GameObject player;

    private LayerMask platformMask;
    private LayerMask wallMask;
    private UnityStandardAssets._2D.UnitParams unitparams;
    static Vector2 toVector2(Vector3 vect)
    {
        Vector2 tRetVec = new Vector2(vect.x, vect.y);
        return tRetVec;
    }
    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
        platformMask = 1 << 20;
        wallMask = 1 << 23;
        patrolWalls = transform.FindChild("PatrolWalls").gameObject;
        unitparams = gameObject.GetComponent<UnityStandardAssets._2D.UnitParams>();
        speed = unitparams.speed;
    }
    public void ChaseEnemy(GameObject enemy)
    {
        player = enemy;
    }

    void Start()
    {
        player = null;
        going_right = true;
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        myWidth = GetComponent<BoxCollider2D>().size.x * gameObject.transform.localScale.x;
        myHeight = GetComponent<BoxCollider2D>().size.y * gameObject.transform.localScale.y;
        created_walls = new ArrayList();

        foreach (Transform wall in patrolWalls.transform)
        {
            GameObject clone = Instantiate(wall.gameObject) as GameObject;
            clone.transform.position = wall.transform.position;
            Vector2 tSize;
            tSize.x = /*wall.transform.localScale.x */ gameObject.transform.localScale.x * patrolWalls.transform.localScale.x;
            tSize.y = /*wall.transform.localScale.x */ gameObject.transform.localScale.y * patrolWalls.transform.localScale.y;
            clone.transform.localScale = tSize;
            clone.layer = 23;
            clone.GetComponent<HeckWall>().myUnit = gameObject;
            created_walls.Add(clone);
            Destroy(wall.gameObject);
        }
        Destroy(patrolWalls);
    }
    void OnDestroy()
    {
        foreach (GameObject wall in created_walls)
        {
            Destroy(wall);
        }
        created_walls.Clear();
    }
    void FixedUpdate()
    {
        bool isGrounded = false;
        bool isBlocked = false;
        Vector2 lineCastPos;
        if (going_right)
        {
            lineCastPos = toVector2(myTrans.position) + toVector2(myTrans.right) * myWidth + Vector2.down * myHeight * 0.8f;
            Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
            Debug.DrawLine(lineCastPos, lineCastPos - toVector2(myTrans.right) * .05f);
        }
        else
        {
            lineCastPos = toVector2(myTrans.position) - toVector2(myTrans.right) * myWidth + Vector2.down * myHeight * 0.8f;
            Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
            Debug.DrawLine(lineCastPos, lineCastPos + toVector2(myTrans.right) * .05f);
        }
        RaycastHit2D tRay = Physics2D.Linecast(lineCastPos, lineCastPos - toVector2(myTrans.right) * .05f, wallMask);
        isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down* myHeight, platformMask);

        if (tRay && tRay.collider.gameObject.GetComponent<HeckWall>().myUnit == gameObject)
        {
            isBlocked = true;
        }
        else
        {
            isBlocked = false;
        }
        m_Anim.SetBool("Crouch", unitparams.croucher);
        m_Anim.SetBool("Ground", isGrounded);
        m_Anim.SetBool("Ground", isGrounded);
        m_Anim.SetFloat("vSpeed", myBody.velocity.y);
        m_Anim.SetFloat("Speed", Mathf.Abs(myBody.velocity.x));
        if (!player)
        {
            if (!isGrounded || isBlocked)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                going_right = !going_right;
            }

            Vector2 myVel = myBody.velocity;
            if (going_right)
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