using UnityEngine;

public class EnemyHitDetectionHeadshot : MonoBehaviour
{
    public EnemyHitDetection mainHealth;
    [HideInInspector] public static int headshots;
    public void Headshot()
    {
        mainHealth.Damage(mainHealth.currentHealth, true); 
        
        Debug.Log("Headshot!");
        headshots++;
        Debug.Log("Total Headshots: " + headshots);
    }
}
