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

    public int totalPoints { get; private set; }

    void Start()
    {
        totalPoints = 0;
        UpdatePointsUI();
    }

    public void AddPoints(int amount)
    {
        totalPoints += amount;
        UpdatePointsUI();
    }

    private void UpdatePointsUI()
    {
        if (pointsText != null)
        {
            pointsText.text = "Points: " + totalPoints;
        }
        else
        {
            Debug.Log($"Added {totalPoints} points. New total: {totalPoints}");
        }
    }
}
