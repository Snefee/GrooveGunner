using UnityEngine;
using UnityEngine.InputSystem;

public class DEBUG_LevelControls : MonoBehaviour
{
    public static DEBUG_LevelControls instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public enum LevelState { NotStarted, InProgress, Finished }
    public LevelState currentState { get; private set; }

    [Header("Dependencies")]
    [Tooltip("The EnemySpawner to control during the level.")]
    public EnemySpawner enemySpawner;

    void Start()
    {
        currentState = LevelState.NotStarted;
        if (enemySpawner == null)
        {
            Debug.LogError("Enemy Spawner is not assigned in the Level Controls!", this);
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // K to start the level
        if (Keyboard.current.kKey.wasPressedThisFrame && currentState == LevelState.NotStarted)
        {
            StartLevel();
        }

        // L to finish the level
        if (Keyboard.current.lKey.wasPressedThisFrame && currentState == LevelState.InProgress)
        {
            FinishLevel();
        }
    }

    public void StartLevel()
    {
        Debug.Log("LEVEL STARTED");
        currentState = LevelState.InProgress;

        if (enemySpawner != null)
        {
            enemySpawner.StartSpawning();
        }
    }

    public void FinishLevel()
    {
        Debug.Log("LEVEL FINISHED");
        currentState = LevelState.Finished;

        if (enemySpawner != null)
        {
            enemySpawner.StopSpawning();
        }
    }
}