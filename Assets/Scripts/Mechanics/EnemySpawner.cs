using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Configuration")]
    public GameObject enemyPrefab;
    public int maxEnemies = 10;
    public float spawnInterval = 2.0f;

    [Header("Spawn Locations")]
    public Transform[] spawnPoints;

    [Header("Target")]
    public Transform playerTransform;

    private int currentEnemyCount;
    private bool isSpawning = false;

    void Start()
    {
        EnemyHitDetection.bodykills = 0;
        EnemyHitDetectionHeadshot.headshots = 0;

        currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in the EnemySpawner! Enemies will not face the player.", this);
        }
    }

    public void StartSpawning()
    {
        if (isSpawning) return;

        Debug.Log("Enemy Spawner has started.", this);
        isSpawning = true;
        InvokeRepeating(nameof(TrySpawnEnemy), 0f, spawnInterval);
    }

    public void StopSpawning()
    {
        if (!isSpawning) return;

        Debug.Log("Enemy Spawner has stopped.", this);
        isSpawning = false;
        CancelInvoke(nameof(TrySpawnEnemy));
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
        if (enemyPrefab == null || playerTransform == null)
        {
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned in EnemySpawner!", this);
            return;
        }

        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // --- Rotation Logic ---
        Vector3 directionToPlayer = playerTransform.position - randomSpawnPoint.position;
        directionToPlayer.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawnPoint.position, lookRotation);

        EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.playerTransform = this.playerTransform;
        }

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
