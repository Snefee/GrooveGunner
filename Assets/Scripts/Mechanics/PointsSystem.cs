using UnityEngine;
using TMPro;

public class PointsSystem : MonoBehaviour
{
    public static PointsSystem instance;

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

    [Header("Point Configuration")]
    public int bodyshotPoints = 1;
    public int headshotPoints = 2;

    [Header("UI")]
    [Tooltip("The TextMeshPro UI element that displays the points.")]
    public TextMeshProUGUI pointsText;
    [Tooltip("The TextMeshPro UI element that displays the accuracy.")]
    public TextMeshProUGUI accuracyText;

    // --- Stats ---
    public int totalPoints { get; private set; }
    public int totalShotsFired { get; private set; }
    public int totalShotsHit { get; private set; }
    public float accuracy => (totalShotsFired > 0) ? ((float)totalShotsHit / totalShotsFired) * 100f : 0f;

    void Start()
    {
        totalPoints = 0;
        totalShotsFired = 0;
        totalShotsHit = 0;
        UpdatePointsUI();
        UpdateAccuracyUI();
    }

    public void AddPoints(int amount)
    {
        totalPoints += amount;
        UpdatePointsUI();
    }

    public void RegisterShotFired()
    {
        totalShotsFired++;
        UpdateAccuracyUI();
    }

    public void RegisterShotHit()
    {
        totalShotsHit++;
        UpdateAccuracyUI();
    }

    private void UpdatePointsUI()
    {
        if (pointsText != null)
        {
            pointsText.text = "Points: " + totalPoints;
        }
    }

    private void UpdateAccuracyUI()
    {
        if (accuracyText != null)
        {
            accuracyText.text = $"Accuracy: {accuracy:F1}%";
        }
    }
}
