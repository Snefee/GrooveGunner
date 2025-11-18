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

    [Header("Gameplay UI")]
    [Tooltip("The parent GameObject for all gameplay UI elements.")]
    public GameObject gameplayUIParent;
    [Tooltip("Displays the points during gameplay.")]
    public TextMeshProUGUI gameplayPointsText;
    [Tooltip("Displays the accuracy during gameplay.")]
    public TextMeshProUGUI gameplayAccuracyText;
    [Tooltip("The heatmap UI that is visible during gameplay.")]
    public HitHeatmap gameplayHeatmap;

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
        if (gameplayHeatmap != null) gameplayHeatmap.UpdateHeatmap(bodyPartHits, totalShotsHit);
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
        if (gameplayHeatmap != null) gameplayHeatmap.UpdateHeatmap(bodyPartHits, totalShotsHit);
    }
    public void SetGameplayUIVisibility(bool isVisible)
    {
        if (gameplayUIParent != null)
        {
            gameplayUIParent.SetActive(isVisible);
        }
    }

    private void UpdatePointsUI()
    {
        if (gameplayPointsText != null) gameplayPointsText.text = "Points: " + totalPoints;
    }

    private void UpdateAccuracyUI()
    {
        if (gameplayAccuracyText != null) gameplayAccuracyText.text = $"Accuracy: {accuracy:F1}%";
    }
}
