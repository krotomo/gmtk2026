using UnityEngine;

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
    }
    public void OnAirPocketEnter(AirPocket pocket)
    {
        isInAirPocket = true;
        currentAirPocket = pocket;
    }
    public void OnAirPocketExit()
    {
        isInAirPocket = false;
        currentAirPocket = null;
    }
}
