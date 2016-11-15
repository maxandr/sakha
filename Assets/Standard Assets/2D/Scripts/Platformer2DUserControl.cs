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
        [HideInInspector] public float axis;
        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
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
            Debug.Log(m_Jump);
        }


        private void FixedUpdate()
        {
            if (m_Character.crouchBlocked) {
                if (!Input.GetKey(KeyCode.Z))
                {
                    m_Character.crouchBlocked = false;
                }
            }
            bool crouch = Input.GetKey(KeyCode.Z);
            axis = CrossPlatformInputManager.GetAxis("Horizontal");
            m_Character.Move(axis, crouch, m_Jump);
        }
    }
}
