﻿using UnityEngine;
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
    public ArrayList created_walls;
    [HideInInspector] public GameObject player;
    public bool isPatroler;
    Vector2 toVector2(Vector3 vect)
    {
        Vector2 tRetVec = new Vector2(vect.x, vect.y);
        return tRetVec;
    }
    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
    }
    public void ChaseEnemy(GameObject enemy) {
        player = enemy;
    }

    void Start()
    {
        player = null;
        going_right = true;
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        myWidth = GetComponent<BoxCollider2D>().size.x;
        myHeight = GetComponent<BoxCollider2D>().size.y;
        created_walls = new ArrayList();
        if (isPatroler)
        {
            foreach (GameObject wall in walls)
            {
                GameObject clone = Instantiate(wall) as GameObject;
                clone.transform.position = wall.transform.position;
                Vector2 tSize = wall.GetComponent<BoxCollider2D>().size;
                tSize.x = tSize.x * gameObject.transform.localScale.x;
                tSize.y = tSize.y * gameObject.transform.localScale.y;
                clone.GetComponent<BoxCollider2D>().size = tSize;
                clone.layer = 23;
                clone.GetComponent<HeckWall>().myHeck = gameObject;
                created_walls.Add(clone);
                Destroy(wall);
            }
        }
        else
        {
            foreach (GameObject wall in walls) {
                Destroy(wall);
            }
        }
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
        isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);

        if (tRay && tRay.collider.gameObject.GetComponent<HeckWall>().myHeck == gameObject)
        {
            isBlocked = true;
        }
        else
        {
            isBlocked = false;
        }
        m_Anim.SetBool("Ground", isGrounded);
        m_Anim.SetFloat("vSpeed", myBody.velocity.y);
        m_Anim.SetFloat("Speed", Mathf.Abs(myBody.velocity.x));
        if (isPatroler && !player)
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

        if (player) {
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
    private void SetAutoFlip() {
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