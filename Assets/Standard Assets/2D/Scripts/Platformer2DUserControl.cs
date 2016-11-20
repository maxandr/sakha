using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private bool m_Shoot;
        private bool m_Punch;
        [HideInInspector] public float axis;
        [HideInInspector] public float axisY;
        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            //Physics2D.IgnoreLayerCollision(21, 22);
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
            if (CrossPlatformInputManager.GetButtonUp("Jump")) {
                m_Jump = false;
                m_Character.stopJumping = true;
            }
            m_Shoot = Input.GetKey(KeyCode.D);
            m_Punch = Input.GetKey(KeyCode.W);
        }


        private void FixedUpdate()
        {
           
            bool crouch = false; ;//= Input.GetKey(KeyCode.Z);
            axis = CrossPlatformInputManager.GetAxis("Horizontal");
            axisY = CrossPlatformInputManager.GetAxisRaw("Vertical");
            if (m_Character.crouchBlocked)
            {
                if (axisY >= 0.0f)
                {
                    m_Character.crouchBlocked = false;
                }
            }
            if (axisY < 0.0f)
            {
                crouch = true;
            }
            m_Character.Move(axis, axisY, crouch, m_Jump);
            if (m_Shoot)
            {
                m_Character.Shoot(CrossPlatformInputManager.GetAxisRaw("Horizontal"), axisY);
            }
            if (m_Punch)
            {
                m_Character.Punch(crouch);
            }
        }
    }
}
