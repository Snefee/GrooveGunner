using UnityEngine;

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

    // Point value configuration
    [Header("Point Configuration")]
    public int bodyshotPoints = 1;
    public int headshotPoints = 2;


    public int totalPoints { get; private set; }

    void Start()
    {
        totalPoints = 0;
        Debug.Log("Total points: " + totalPoints);
    }

    public void AddPoints(int amount)
    {
        totalPoints += amount;
        Debug.Log($"Added {amount} points. New total: {totalPoints}");
    }
}
