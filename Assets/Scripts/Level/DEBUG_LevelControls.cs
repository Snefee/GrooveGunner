using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DEBUG_LevelControls : MonoBehaviour
{
    public static DEBUG_LevelControls instance;

    InputAction pauseAction;

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

    public enum LevelState { NotStarted, InProgress, Paused, Finished }
    public LevelState currentState { get; private set; }

    [Header("Dependencies")]
    [Tooltip("The EnemySpawner to control during the level.")]
    public EnemySpawner enemySpawner;
    [Tooltip("The UI panel to show when the level is finished.")]
    public GameObject levelEndCard;
    [Tooltip("The UI panel for the pause menu.")]
    public GameObject pauseMenu;
    [Tooltip("The PlayerInput component on the player character.")]
    public PlayerInput playerInput;

    [Header("End Card UI")]
    [Tooltip("The TextMeshPro UI element on the end card for displaying final points.")]
    public TextMeshProUGUI endCardPointsText;
    [Tooltip("The TextMeshPro UI element on the end card for displaying final accuracy.")]
    public TextMeshProUGUI endCardAccuracyText;
    [Tooltip("The heatmap UI that is part of the Level End Card.")]
    public HitHeatmap endCardHeatmap;


    void Start()
    {
        currentState = LevelState.NotStarted;

        pauseAction = InputSystem.actions.FindAction("Pause");

        if (enemySpawner == null)
        {
            Debug.LogError("Enemy Spawner is not assigned in the Level Controls!", this);
        }

        if (levelEndCard == null)
        {
            Debug.LogError("Level End Card is not assigned in the Level Controls!", this);
        }
        if (pauseMenu == null)
        {
            Debug.LogError("Pause Menu is not assigned in the Level Controls!", this);
        }
        if (endCardHeatmap == null)
        {
            Debug.LogError("End Card Heatmap is not assigned in the Level Controls!", this);
        }
        if (playerInput == null)
        {
            Debug.LogError("Player Input is not assigned in the Level Controls!", this);
        }
        if (endCardPointsText == null)
        {
            Debug.LogError("End Card Points Text is not assigned in the Level Controls!", this);
        }
        if (endCardAccuracyText == null)
        {
            Debug.LogError("End Card Accuracy Text is not assigned in the Level Controls!", this);
        }
        
        if (levelEndCard != null)
        {
            levelEndCard.SetActive(false);
        }
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        if (playerInput != null) playerInput.enabled = false;

        PointsSystem.instance.SetGameplayUIVisibility(false);
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

        // Toggle pause
        if (pauseAction.WasPressedThisFrame())
        {
            if (currentState == LevelState.InProgress)
            {
                PauseGame();
            }
            else if (currentState == LevelState.Paused)
            {
                ResumeGame();
            }
        }
    }

    public void StartLevel()
    {
        Debug.Log("LEVEL STARTED");
        currentState = LevelState.InProgress;

        if (playerInput != null) playerInput.enabled = true;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (enemySpawner != null)
        {
            enemySpawner.StartSpawning();
        }

        PointsSystem.instance.SetGameplayUIVisibility(true);
    }

    public void FinishLevel()
    {
        Debug.Log("LEVEL FINISHED");
        currentState = LevelState.Finished;

        if (playerInput != null) playerInput.enabled = false;

        if (enemySpawner != null)
        {
            enemySpawner.StopSpawning();
        }

        PointsSystem.instance.SetGameplayUIVisibility(false);

        if (endCardHeatmap != null)
        {
            endCardHeatmap.UpdateHeatmap(PointsSystem.instance.bodyPartHits, PointsSystem.instance.totalShotsHit);
        }

        if (endCardPointsText != null)
        {
            endCardPointsText.text = "Total Points: " + PointsSystem.instance.totalPoints;
        }
        if (endCardAccuracyText != null)
        {
            endCardAccuracyText.text = $"Final Accuracy: {PointsSystem.instance.accuracy:F1}%";
        }

        if (levelEndCard != null)
        {
            levelEndCard.SetActive(true);
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void PauseGame()
    {
        Debug.Log("GAME PAUSED");
        currentState = LevelState.Paused;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (playerInput != null) playerInput.enabled = false;

        if (pauseMenu != null) pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Debug.Log("GAME RESUMED");
        currentState = LevelState.InProgress;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (playerInput != null) playerInput.enabled = true;

        if (pauseMenu != null) pauseMenu.SetActive(false);
    }
}