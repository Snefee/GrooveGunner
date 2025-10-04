using UnityEngine;

public class EnemyHitDetection : MonoBehaviour
{
    public int currentHealth = 100;

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)

        {
            gameObject.SetActive(false);

        }
    }
}
