using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce_min = 10f;                  // Amount of force added when the player jumps.
        [SerializeField] private float mJump_Max_timer = 0.5f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        public GameObject bullet_instantiate;
        public GameObject bullet;
        public float bulletSpeed = 50.0f;
        public float fireRate = 0.0F;
        private float nextFire = 0.0F;
        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        [HideInInspector]
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private float jump_timer;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        //Punching
        public GameObject PunchCollider;
        [SerializeField]
        private float punch_timer;
        private float punch_timer_curr;
        private bool punching = false;
        //
        [HideInInspector]
        public bool stopJumping = false;
        [HideInInspector]
        public bool crouchBlocked = false;
        public float teleport_cd;
        public float force;
        private float teleport_timer;
        [HideInInspector]
        public bool teleporting;
        private float teleporting_timer;
        public float teleporting_cd;
        private bool teleport_blocked;
        private BoxCollider2D hitcollider;
        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            hitcollider = transform.FindChild("EnemyHitCollider").GetComponent<BoxCollider2D>();
            jump_timer = 0.0f;
			nextFire = Time.time;
            punch_timer_curr = 0.0f;
            teleport_timer = teleport_cd;
            teleporting = false;
            m_MaxSpeed = gameObject.GetComponent<UnitParams>().speed;
        }

        public void StopTeleport()
        {
            teleporting = false;
            Vector2 tVec = new Vector2(0.0f, 0.0f);
            m_Rigidbody2D.velocity = tVec;
        }

        private void FixedUpdate()
        {

            if (hitcollider.IsTouchingLayers(m_WhatIsGround))
            {
                teleport_blocked = true;
            }
            else
            {
                teleport_blocked = false;
            }
            Vector2 lineCastPos = toVector2(transform.position) ;
            Debug.DrawLine(lineCastPos, lineCastPos + toVector2(transform.right) * 2);

            m_Grounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    if (stopJumping)
                    {
                        jump_timer = 0.0f;
                    }

                    stopJumping = false;
                }
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);


            teleport_timer += Time.deltaTime;
            if (teleport_timer >= teleport_cd)
            {
                teleport_timer = teleport_cd;
            }
            if (teleporting)
            {
                teleporting_timer += Time.deltaTime;
                float tForce = force;
                if (!m_FacingRight) {
                    tForce *= -1;
                }

                m_Rigidbody2D.AddForce(transform.right * tForce);
                if (teleporting_timer >= teleporting_cd)
                {
                    StopTeleport();
                }
            }
        }


        public void Move(float move, float axisY, bool crouch, bool jump)
        {
            if (!crouch && m_Anim.GetBool("Crouch") && !crouchBlocked)
            {
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            if (!crouchBlocked)
            {
                m_Anim.SetBool("Crouch", crouch);
                if (!m_Grounded) {
                    m_Anim.SetBool("Crouch", false);
                }

            }
            if (m_Grounded || m_AirControl)
            {
                move = (crouch&& !crouchBlocked ? move*m_CrouchSpeed : move);

                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                if (move > 0 && !m_FacingRight)
                {
                    Flip();
                }
                else if (move < 0 && m_FacingRight)
                {
                    Flip();
                }
            }
            if (/*m_Grounded &&*/ jump/* && m_Anim.GetBool("Ground")*/)
            {
                jump_timer += Time.deltaTime;
                if (jump_timer <= mJump_Max_timer && !stopJumping)
                {
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce_min);
                }
                // m_Grounded = false;
                m_Anim.SetBool("Ground", m_Grounded);

            }

            if (punching) {
                punch_timer_curr += Time.deltaTime;
                if (punch_timer_curr >= punch_timer) {
                    punching = false;
                    punch_timer_curr = 0.0f;
                    PunchCollider.GetComponent<PolygonCollider2D>().enabled = false;
                    PunchCollider.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

        }
        static int Sign(float number)
        {
            return number < 0 ? -1 : (number > 0 ? 1 : 0);
        }
        public void Shoot(float axisX,float axisY)
        {
            if (nextFire <= Time.time && !teleporting)
            {
                Debug.Log(axisY);
                nextFire = Time.time + fireRate;
                int axises = (axisY == 0.0f ? 0 : Sign(axisY));
                if (axisX <=0.2f && axisX >= -0.2f)
                {
                    axises *= 2;
                }
                float speeddec = 1.0f;
                if (axises != 0)
                {
                    speeddec = 1.0f / Mathf.Sqrt(2);
                }
                if(axisY==0.0f)
                {
                    axisX = 1;
                }
                if (m_Anim.GetBool("Crouch"))
                {
                    axises = 0;
                    axisX = 1.0f;
                }
                if (m_FacingRight)
                {
                    GameObject clone = Instantiate(bullet, bullet_instantiate.transform.position, bullet_instantiate.transform.rotation) as GameObject;
                    clone.transform.Rotate(new Vector3(0, 0, 45 * axises));
                    clone.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * speeddec * Mathf.Abs(axisX), bulletSpeed * axises * speeddec);
                }
                else
                {
                    GameObject clone = Instantiate(bullet, bullet_instantiate.transform.position, bullet_instantiate.transform.rotation) as GameObject;
                    clone.transform.Rotate(new Vector3(0, 180, 45 * axises));
                    clone.GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletSpeed * speeddec * Mathf.Abs(axisX), bulletSpeed * axises * speeddec);
                }
            }
        }
        public void Punch(bool crouch)
        {
            if (nextFire <= Time.time && !punching && !teleporting)
            {
                if (!punching)
                {
                    if (crouch)
                    {
                        if (!Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                        {
                            nextFire = Time.time + fireRate;
                            punch_timer_curr = 0.0f;
                            PunchCollider.GetComponent<PolygonCollider2D>().enabled = true;
                            PunchCollider.GetComponent<SpriteRenderer>().enabled = true;
                            punching = true;
                            crouchBlocked = true;
                            crouch = false;
                            m_Anim.SetBool("Crouch", false);
                        }
                    }
                    else
                    {
                        nextFire = Time.time + fireRate;
                        punch_timer_curr = 0.0f;
                        PunchCollider.GetComponent<PolygonCollider2D>().enabled = true;
                        PunchCollider.GetComponent<SpriteRenderer>().enabled = true;
                        punching = true;
                    }
                }
            }
        }
        private void Flip()
        {
            m_FacingRight = !m_FacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        Vector2 toVector2(Vector3 vect)
        {
            Vector2 tRetVec = new Vector2(vect.x, vect.y);
            return tRetVec;
        }
        public void Teleport() {
            if(teleport_blocked || teleport_timer < teleport_cd || m_Anim.GetBool("Crouch"))
            {
                return;
            }
            teleport_timer = 0.0f;
            teleporting_timer = 0.0f;
            teleporting = true;
        }
    }
}
