using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirDisplay : MonoBehaviour
{
    [SerializeField] private Air playerAir;
    [SerializeField] private Image airFillImage;
    [SerializeField] private AirDisplayColors airDisplayColors;
    [SerializeField] private ParticleSystem bubbleParticleSystem;
    [SerializeField] private float maxBubbleEmissionRate = 20f;

    private RectTransform bubbleRectTransform;
    private ParticleSystemRenderer bubbleRenderer;

    private void Awake()
    {
        bubbleRectTransform = bubbleParticleSystem != null ? bubbleParticleSystem.GetComponent<RectTransform>() : null;
        bubbleRenderer = bubbleParticleSystem != null ? bubbleParticleSystem.GetComponent<ParticleSystemRenderer>() : null;

        if (bubbleRenderer != null)
        {
            bubbleRenderer.sortingOrder = 20;
        }

        ConfigureBubbleSystem();
    }

    private void Start()
    {
        if (playerAir != null)
        {
            playerAir.EventAirChanged += UpdateAirVisual;
        }

        UpdateAirVisual();
    }

    private void LateUpdate()
    {
        if (airFillImage == null || bubbleRectTransform == null)
        {
            return;
        }

        if (bubbleRectTransform.parent != airFillImage.rectTransform)
        {
            bubbleRectTransform.SetParent(airFillImage.rectTransform, false);
            bubbleRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            bubbleRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            bubbleRectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        float fillAmount = Mathf.Clamp01(airFillImage.fillAmount);
        float barHeight = airFillImage.rectTransform.rect.height;

        // This moves the emitter downward as the fill amount decreases.
        float yOffset = barHeight * 0.5f - (barHeight * (1f - fillAmount));

        bubbleRectTransform.anchoredPosition = new Vector2(0f, yOffset);
    }

    private void ConfigureBubbleSystem()
    {
        if (bubbleParticleSystem == null)
        {
            return;
        }

        var main = bubbleParticleSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.8f, 1.6f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.08f, 0.18f);

        var velocity = bubbleParticleSystem.velocityOverLifetime;
        velocity.enabled = true;
        velocity.x = new ParticleSystem.MinMaxCurve(-0.2f, 0.2f);
        velocity.y = new ParticleSystem.MinMaxCurve(0.6f, 1.2f);

        var emission = bubbleParticleSystem.emission;
        emission.enabled = true;
        emission.rateOverTime = 0f;
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

        SetBubbleIntensity(playerAir.GetAirDrainIntensity());
    }

    private void SetBubbleIntensity(float intensity)
    {
        if (bubbleParticleSystem == null)
        {
            return;
        }

        var emission = bubbleParticleSystem.emission;

        float baseRate = 3f;
        float boostedRate = baseRate + (intensity * maxBubbleEmissionRate);

        float jitter = Mathf.Lerp(0.8f, 1.2f, Mathf.PerlinNoise(Time.time * 2f, 0.25f));

        emission.rateOverTime = boostedRate * jitter;

        if (boostedRate <= 0.01f)
        {
            bubbleParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        else if (!bubbleParticleSystem.isPlaying)
        {
            bubbleParticleSystem.Play();
        }
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
