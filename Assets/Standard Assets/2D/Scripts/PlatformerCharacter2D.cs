using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
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
        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            jump_timer = 0.0f;
			nextFire = Time.time;
            punch_timer_curr = 0.0f;
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
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
        }


        public void Move(float move, float axisY, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch") && !crouchBlocked)
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            if (!crouchBlocked)
            {
                m_Anim.SetBool("Crouch", crouch);
            }
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch&& !crouchBlocked ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            if (/*m_Grounded &&*/ jump/* && m_Anim.GetBool("Ground")*/)
            {
                // Add a vertical force to the player.
                jump_timer += Time.deltaTime;
                if (jump_timer <= mJump_Max_timer && !stopJumping)
                {
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce_min);
                }
                // m_Grounded = false;
                m_Anim.SetBool("Ground", false);

            }

            if (punching) {
                punch_timer_curr += Time.deltaTime;
                if (punch_timer_curr >= punch_timer) {
                    punching = false;
                    punch_timer_curr = 0.0f;
                    PunchCollider.GetComponent<PolygonCollider2D>().enabled = false;
                }
            }

        }
        static int Sign(float number)
        {
            return number < 0 ? -1 : (number > 0 ? 1 : 0);
        }
        public void Shoot(float axisX,float axisY)
        {
            if (nextFire <= Time.time)
            {
                nextFire = Time.time + fireRate;
                int axises = (axisY == 0.0f ? 0 : Sign(axisY));
                if (axisX == 0.0f)
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
            if (nextFire <= Time.time && !punching)
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
                        punching = true;
                    }
                }
            }
        }
        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
