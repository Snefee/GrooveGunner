using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HitHeatmap : MonoBehaviour
{
    [System.Serializable]
    public struct BodyPartUI
    {
        public BodyPart bodyPart;
        public Image image;
        public TextMeshProUGUI percentageText;
    }

    public BodyPartUI[] bodyPartUIs;
    public Color baseColor = Color.white;
    public Color maxHitColor = Color.red;

    public void UpdateHeatmap(Dictionary<BodyPart, int> hitData, int totalHits)
    {
        foreach (var ui in bodyPartUIs)
        {
            if (hitData.ContainsKey(ui.bodyPart))
            {
                int hits = hitData[ui.bodyPart];
                float percentage = (totalHits > 0) ? ((float)hits / totalHits) : 0;

                // Update the color based on the percentage of hits
                ui.image.color = Color.Lerp(baseColor, maxHitColor, percentage);

                // Update the text to show the percentage
                ui.percentageText.text = $"{percentage * 100:F0}%";
            }
        }
    }
}
