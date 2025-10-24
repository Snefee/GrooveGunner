using JetBrains.Annotations;
using UnityEngine;

public class EnemyHitDetection : MonoBehaviour
{
    public EnemySpawner countEnemy;
    public int currentHealth = 100;
    [HideInInspector] public static int bodykills;
    private bool isDead = false;

    public void Damage(int damageAmount, bool isHeadshot = false)
    {
        if (isDead == true) return;

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            isDead = true;
            Debug.Log("Enemy defeated!", this.gameObject);

            if (!isHeadshot)
            {
                bodykills++;
                Debug.Log("Total Bodyshot kills: " + bodykills);
                
                PointsSystem.instance.AddPoints(PointsSystem.instance.bodyshotPoints);
            }

            if (countEnemy != null)
            {
                countEnemy.EnemyDefeated();
            }
            else
            {
                Debug.LogWarning("Enemy was not linked to a spawner. Count may be inaccurate.", this.gameObject);
            }

            gameObject.SetActive(false);
        }
    }
}
