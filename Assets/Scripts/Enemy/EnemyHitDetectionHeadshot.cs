using UnityEngine;

public class EnemyHitDetectionHeadshot : MonoBehaviour
{
    public EnemyHitDetection mainHealth;
    public void Headshot()
    {
        mainHealth.Damage(mainHealth.currentHealth);
    }
}
