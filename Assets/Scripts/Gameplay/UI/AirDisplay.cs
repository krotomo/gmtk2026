using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AirDisplay : MonoBehaviour
{
    [SerializeField] Air playerAir;
    [SerializeField] Image airFillImage;
    [SerializeField] AirDisplayColors airDisplayColors;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAir.EventAirChanged += UpdateAirVisual;
    }

    void UpdateAirVisual()
    {
        airFillImage.fillAmount = playerAir.GetAirPercent();
    }
}

[System.Serializable]
struct AirDisplayColors
{
    public List<AirDisplayColor> airDisplayColors;
}

[System.Serializable]
struct AirDisplayColor
{
    public string name;
    public Color32 color;
    public float percentThreshold;
}
