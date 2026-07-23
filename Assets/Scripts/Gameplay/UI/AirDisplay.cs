using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirDisplay : MonoBehaviour
{
    [SerializeField] private Air playerAir;
    [SerializeField] private Image airFillImage;
    [SerializeField] private AirDisplayColors airDisplayColors;

    private void Start()
    {
        if (playerAir != null)
        {
            playerAir.EventAirChanged += UpdateAirVisual;
        }

        UpdateAirVisual();
    }

    private void UpdateAirVisual()
    {
        if (playerAir == null || airFillImage == null)
        {
            return;
        }

        float percent = Mathf.Clamp01(playerAir.GetAirPercent());
        airFillImage.fillAmount = percent;
        airFillImage.color = GetColorForPercent(percent);
    }

    private Color GetColorForPercent(float percent)
    {
        if (airDisplayColors.airDisplayColors == null || airDisplayColors.airDisplayColors.Count == 0)
        {
            return Color.white;
        }

        percent = Mathf.Clamp01(percent);

        List<AirDisplayColor> colors = airDisplayColors.airDisplayColors;
        colors.Sort((a, b) => a.percentThreshold.CompareTo(b.percentThreshold));

        if (percent <= colors[0].percentThreshold)
        {
            return colors[0].color;
        }

        for (int i = 1; i < colors.Count; i++)
        {
            if (percent <= colors[i].percentThreshold)
            {
                float previousThreshold = colors[i - 1].percentThreshold;
                float nextThreshold = colors[i].percentThreshold;

                float t = Mathf.InverseLerp(previousThreshold, nextThreshold, percent);
                t = Mathf.SmoothStep(0f, 1f, t);

                return Color.Lerp(colors[i - 1].color, colors[i].color, t);
            }
        }

        return colors[colors.Count - 1].color;
    }
}

[System.Serializable]
public struct AirDisplayColors
{
    public List<AirDisplayColor> airDisplayColors;
}

[System.Serializable]
public struct AirDisplayColor
{
    public string name;
    public Color color;
    public float percentThreshold;
}
