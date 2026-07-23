using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Fields related to input bindings and lookup
    [SerializeField] InputActionAsset inputActionAsset;
    InputAction KickLeft;
    InputAction KickRight;
    InputAction Look;
    // Fields for reference to relevant rigidbodies
    [SerializeField] private Rigidbody2D torsoRb;
    [SerializeField] private Rigidbody2D leftUpperLegRb;
    [SerializeField] private Rigidbody2D rightUpperLegRb;

    //Fields related to game state
    //This one is for resets if we end up modifying the maximum for a leg due to overuse
    [SerializeField] float legsMaxChargeRate;
    [SerializeField] float legsMaxChargeValue;
    [SerializeField] float legsTimeToMaxCharge;

    //Fields related to force applied when kicking
    [SerializeField] private float kickImpulsePerUnitCharge = 10f;
    [SerializeField] private float kickBaseForce = 6f;
    [SerializeField] private float kickBaseTorque = 90f;
    [SerializeField] private float kickOffset = 0.5f;

    //Left leg first
    float leftLegCharge;
    float leftLegMaxCharge;
    float leftLegChargeRate;
    //Now the right
    float rightLegCharge;
    float rightLegMaxCharge;
    float rightLegChargeRate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(inputActionAsset != null){
            KickLeft = inputActionAsset.FindAction("KickLeft");
            KickRight = inputActionAsset.FindAction("KickRight");
            Look = inputActionAsset.FindAction("Look");
            KickLeft.Enable();
            KickRight.Enable();
            Look.Enable();
        }
        ResetLegChargeValues();
        GameManager.instance.EventPlaying += HandleInput;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleInput()
    {
        DetectLegs();
    }

    //Helpers
    void ResetLegChargeValues()
    {
        //Only for use when needing to fix the max values instantly, such as on restart
        rightLegMaxCharge = legsMaxChargeValue;
        leftLegMaxCharge = legsMaxChargeValue;
        rightLegChargeRate = legsMaxChargeRate;
        leftLegChargeRate = legsMaxChargeRate;
    }
    void DetectLegs()
    {
        //Left leg first
        //If the left kick button is pressed, add to charge
        if (KickLeft.IsPressed())
        {
            leftLegCharge = Mathf.Clamp(leftLegCharge + (leftLegChargeRate * Time.deltaTime), 0, leftLegMaxCharge);
        }
        //If left kick is not pressed, fire off the kick based on the charge, if there is any charge
        else if(leftLegCharge > 0)
        {
            ApplyKick(true);
        }
        //Then the right
        if (KickRight.IsPressed())
        {
            rightLegCharge = Mathf.Clamp(rightLegCharge + (rightLegChargeRate * Time.deltaTime), 0, rightLegMaxCharge);
        }
        else if(rightLegCharge > 0)
        {
            ApplyKick(false);
        }
    }

    void ApplyKick(bool isLeftLeg)
    {
        Rigidbody2D legRb = isLeftLeg ? leftUpperLegRb : rightUpperLegRb;
        float chargeRatio = isLeftLeg
            ? leftLegCharge / Mathf.Max(1f, leftLegMaxCharge)
            : rightLegCharge / Mathf.Max(1f, rightLegMaxCharge);

        float forceAmount = kickBaseForce + (chargeRatio * kickImpulsePerUnitCharge);
        float torqueAmount = kickBaseTorque * chargeRatio;

        Vector2 kickDirection = (Vector2)legRb.transform.right;
        Vector2 applyPoint = (Vector2)legRb.transform.position + kickDirection * kickOffset;

        legRb.AddForceAtPosition(kickDirection * forceAmount, applyPoint, ForceMode2D.Impulse);
        legRb.AddTorque(isLeftLeg ? -torqueAmount : torqueAmount, ForceMode2D.Impulse);

        if (isLeftLeg)
            leftLegCharge = 0f;
        else
            rightLegCharge = 0f;
    }
}
