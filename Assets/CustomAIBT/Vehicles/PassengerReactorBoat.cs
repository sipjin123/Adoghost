using UnityEngine;

public class PassengerReactorBoat : MonoBehaviour
{
    [Header("Motion Multipliers")]
    public float swayMultiplier = 1f;    // Multiplies boat tilt (Z)
    public float nodMultiplier = 5f;     // Multiplies boat bob (Y)

    [Header("Random Variation Ranges")]
    public float swayVariance = 0.2f;
    public float nodVariance = 0.5f;
    public float phaseOffsetRange = 0.5f; // Time phase shift

    private Quaternion initialLocalRotation;
    private float swayMultiplierFinal;
    private float nodMultiplierFinal;
    private float phaseOffset;

    void Start()
    {
        initialLocalRotation = transform.localRotation;

        // Apply random per-instance variation
        swayMultiplierFinal = swayMultiplier * Random.Range(1f - swayVariance, 1f + swayVariance);
        nodMultiplierFinal = nodMultiplier * Random.Range(1f - nodVariance, 1f + nodVariance);
        phaseOffset = Random.Range(-phaseOffsetRange, phaseOffsetRange);
    }

    public void ReactToBoatMotion(float verticalOffset, float rollAngle)
    {
        float adjustedTime = Time.time + phaseOffset;

        // Add subtle wobble using phase offset (optional enhancement)
        float extraWobble = Mathf.Sin(adjustedTime * 3f) * 0.5f;

        float nodAngle = (verticalOffset + extraWobble * 0.01f) * nodMultiplierFinal;
        float swayAngle = (rollAngle + extraWobble * 0.5f) * swayMultiplierFinal;

        Quaternion nod = Quaternion.Euler(nodAngle, 0f, 0f);
        Quaternion sway = Quaternion.Euler(0f, 0f, swayAngle);

        transform.localRotation = initialLocalRotation * nod * sway;
    }
    /* OLD
    [Header("Sway Reaction")] public float swayAmplitude = 0.05f;
    public float swayFrequency = 1f;

    [Header("Head Bobbing")] public float headBobAmplitude = 0.05f;
    public float headBobFrequency = 1.2f;

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }

    public void ReactToBump(float time)
    {
        // Gentle side sway (on Z-axis)
        float sway = Mathf.Sin(time * swayFrequency) * swayAmplitude;
        Quaternion swayTilt = Quaternion.Euler(0f, 0f, sway * Mathf.Rad2Deg);

        // Slight up-down motion (head bobbing)
        float bob = Mathf.Sin(time * headBobFrequency) * headBobAmplitude;
        Vector3 bobOffset = new Vector3(0f, bob, 0f);

        // Apply combined effect
        transform.localPosition = initialLocalPosition + bobOffset;
        transform.localRotation = initialLocalRotation * swayTilt;
    }*/
}