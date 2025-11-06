using UnityEngine;
using TMPro;
using System.Collections.Generic;

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
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI accuracyText;
    public HitHeatmap hitHeatmap;

    // --- Stats ---
    public int totalPoints { get; private set; }
    public int totalShotsFired { get; private set; }
    public int totalShotsHit { get; private set; }
    public float accuracy => (totalShotsFired > 0) ? ((float)totalShotsHit / totalShotsFired) * 100f : 0f;

    public Dictionary<BodyPart, int> bodyPartHits { get; private set; } = new Dictionary<BodyPart, int>();

    void Start()
    {
        totalPoints = 0;
        totalShotsFired = 0;
        totalShotsHit = 0;

        foreach (BodyPart bp in System.Enum.GetValues(typeof(BodyPart)))
        {
            bodyPartHits[bp] = 0;
        }

        UpdatePointsUI();
        UpdateAccuracyUI();
        if (hitHeatmap != null) hitHeatmap.UpdateHeatmap(bodyPartHits, totalShotsHit);
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

    public void RegisterHit(BodyPart bodyPart)
    {
        totalShotsHit++;
        if (bodyPartHits.ContainsKey(bodyPart))
        {
            bodyPartHits[bodyPart]++;
        }
        
        UpdateAccuracyUI();
        if (hitHeatmap != null) hitHeatmap.UpdateHeatmap(bodyPartHits, totalShotsHit);
    }

    private void UpdatePointsUI()
    {
        if (pointsText != null) pointsText.text = "Points: " + totalPoints;
    }

    private void UpdateAccuracyUI()
    {
        if (accuracyText != null) accuracyText.text = $"Accuracy: {accuracy:F1}%";
    }
}
