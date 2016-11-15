using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        [HideInInspector] public BoxCollider2D coll;
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
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            axis = CrossPlatformInputManager.GetAxis("Horizontal");
            m_Character.Move(axis, crouch, m_Jump);
        }
    }
}
