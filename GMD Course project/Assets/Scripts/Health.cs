using System;
using UnityEngine;  

    public class Health : MonoBehaviour
    {
        public float currentHealth;
        public float maximumHealth=2;


        private void Start()
        {
            currentHealth = maximumHealth;
        }

        public void TakeDamage(float damageAmount)
        {
            currentHealth -= damageAmount;
            if (currentHealth<=0)
            {
                Die();
            }
        }

        private void Die()
        {
          Destroy(gameObject);
        }
    }
