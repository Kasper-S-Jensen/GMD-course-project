using System.Collections;
using UnityEngine;

public class HealingArea : MonoBehaviour
{
    public float healingAmount;
    public float healingTime;

    private IEnumerator healingCoroutine;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healingCoroutine = StartHealing();
            StartCoroutine(healingCoroutine);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(healingCoroutine);
        }
    }

    private IEnumerator StartHealing()
    {
        while (true)
        {
            yield return new WaitForSeconds(healingTime);
            // checks if the player object has player health
            var playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.StartHealing(healingAmount);
            }
        }
    }
}