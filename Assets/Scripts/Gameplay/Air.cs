using UnityEngine;
using System;

public class Air : MonoBehaviour
{
    [SerializeField] float maximumAirCapacity = 100f;
    float currentAir;
    bool isInAirPocket;
    AirPocket currentAirPocket = null;
    //Field for the default amount of air lost
    [SerializeField] float airDecayAmount = 1.0f;
    //Field for modifiers from excessive movement, or other effects, on air consumption
    [SerializeField] float airDecayModifier = 0f;
    [SerializeField] float airGainAmount = 10.0f;
    public event Action EventAirChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAir = maximumAirCapacity;
        GameManager.instance.EventPlaying += ModifyAirValue;
    }

    //Hooked this into the gamemanager, will only run while in playing state.
    void ModifyAirValue()
    {
        if (isInAirPocket)
        {
            currentAir = Mathf.Clamp(currentAir + currentAirPocket.ConsumeAirFromPocket(airGainAmount * Time.deltaTime), 0f, maximumAirCapacity);
        }
        else
        {
            //Controls for framerate in air consumption
            currentAir -= ((airDecayAmount + airDecayModifier) * Time.deltaTime);
        }
        OnAirChanged();
    }

    void OnAirChanged()
    {
        EventAirChanged?.Invoke();
    }

    //Exposed for UI
    public float GetAirPercent()
    {
        return (currentAir / maximumAirCapacity);
    }
    
    public float GetAirDrainIntensity()
    {
        if (isInAirPocket)
        {
            return 0f;
        }

        float effectiveDrain = airDecayAmount + airDecayModifier;
        float baseDrain = airDecayAmount;

        if (effectiveDrain <= baseDrain)
        {
            return 0f;
        }

        return Mathf.Clamp01((effectiveDrain - baseDrain) / Mathf.Max(0.0001f, baseDrain));
    }

    //Tracking air pocket occupancy
    public void OnAirPocketEnter(AirPocket pocket)
    {
        Debug.Log("I entered an air pocket.");
        isInAirPocket = true;
        currentAirPocket = pocket;
    }
    public void OnAirPocketExit()
    {
        Debug.Log("I exited an air pocket.");
        isInAirPocket = false;
        currentAirPocket = null;
    }
}
