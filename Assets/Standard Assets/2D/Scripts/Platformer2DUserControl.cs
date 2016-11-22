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
        private bool m_Teleport;
        [HideInInspector] public float axis;
        [HideInInspector] public float axisY;
        private bool crouch;
        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            crouch = false;
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
            m_Shoot =( CrossPlatformInputManager.GetButtonDown("Fire1") || CrossPlatformInputManager.GetAxisRaw("Fire1")!=0.0f);
            m_Punch = (CrossPlatformInputManager.GetButtonDown("Fire2") || CrossPlatformInputManager.GetAxisRaw("Fire2") != 0.0f);
            m_Teleport = CrossPlatformInputManager.GetButtonDown("Teleport");
        }


        private void FixedUpdate()
        {
            bool tOldCrouch = crouch;
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
            else
            {
                crouch = false;
            }
            if (m_Shoot || m_Punch)
            {
                if (!tOldCrouch)
                {
                    crouch = false;
                }
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
            if (m_Teleport)
            {
                m_Character.Teleport();
                m_Teleport = false;
            }
        }
    }
}
