using UnityEngine;

public class AirPocket : MonoBehaviour
{
    [SerializeField] float pocketMaxAirCapacity = 500f;
    float pocketCurAirCapacity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pocketCurAirCapacity = pocketMaxAirCapacity;
    }

    public float ConsumeAirFromPocket(float amount)
    {
        if(pocketCurAirCapacity > 0)
        {
            if(pocketCurAirCapacity >= amount)
            {
                pocketCurAirCapacity -= amount;
                return amount;
            }
            else
            {
                amount = pocketCurAirCapacity;
                pocketCurAirCapacity = 0;
                return amount;
            }
        }else return 0;
    }
}
