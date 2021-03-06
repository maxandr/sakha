﻿using UnityEngine;
using System.Collections;
namespace UnityStandardAssets._2D
{
    public class UnitParams : MonoBehaviour
    {
        public float speed;
        public float max_health;
        public float punch_cd;
        public float shoot_cd;
        public float shoot_dmg;
        public float punch_duration;
        public int punch_dmg;
        public bool croucher;//temp
        [HideInInspector]
        public float current_health;
        public bool immortal;
        void Awake()
        {
            current_health = max_health;
        }
        public void Hitted(int dmg)
        {
            if (immortal) {
                return;
            }

            current_health -= dmg;
            if (current_health <= 0)
            {
                if (gameObject.tag == "Player")
                {
                    Application.LoadLevel(Application.loadedLevel);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

}