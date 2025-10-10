using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Configuration")]
    public GameObject enemyPrefab;
    public int maxEnemies = 10;
    public float spawnInterval = 2.0f;

    [Header("Spawn Locations")]
    public Transform[] spawnPoints;

    private int currentEnemyCount;

    void Start()
    {
        EnemyHitDetection.bodykills = 0;
        EnemyHitDetectionHeadshot.headshots = 0;
        currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        InvokeRepeating(nameof(TrySpawnEnemy), spawnInterval, spawnInterval);
    }

    void TrySpawnEnemy()
    {
        if (currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab is not assigned in the EnemySpawner!");
            return;
        }
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned in the EnemySpawner!");
            return;
        }

        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);

        EnemyHitDetection enemyHealth = newEnemy.GetComponent<EnemyHitDetection>();
        if (enemyHealth != null)
        {
            enemyHealth.countEnemy = this;
        }

        currentEnemyCount++;
    }

    public void EnemyDefeated()
    {
        currentEnemyCount--;
        Debug.Log("An enemy was defeated. Enemies remaining: " + currentEnemyCount);
    }
}
