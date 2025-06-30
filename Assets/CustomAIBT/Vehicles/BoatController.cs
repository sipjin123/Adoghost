using UnityEngine;

public class BoatMover : MonoBehaviour
{
    [Header("Waypoint Navigation")]
    public Transform[] waypoints;
    public float speed = 5f;
    public float reachThreshold = 1f;
    public float turnSmoothness = 2f;

    [Header("Water Motion")]
    public Transform boatBody;
    public float waveHeight = 0.4f;
    public float waveFrequency = 1.5f;
    public float tiltAngle = 10f;
    public float tiltSpeed = 1.5f;

    private int currentIndex = 0;
    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;
    private PassengerReactorBoat[] passengers;

    void Start()
    {
        if (boatBody != null)
        {
            initialLocalPos = boatBody.localPosition;
            initialLocalRot = boatBody.localRotation;
            passengers = boatBody.GetComponentsInChildren<PassengerReactorBoat>();
        }
    }

    void Update()
    {
        if (waypoints.Length == 0 || boatBody == null) return;

        // Move toward current waypoint
        Vector3 targetPos = waypoints[currentIndex].position;
        Vector3 direction = (targetPos - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;

        // Smoothly rotate to face movement direction
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSmoothness);
        }

        // Advance to next waypoint if close enough
        if (Vector3.Distance(transform.position, targetPos) < reachThreshold)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }

        AnimateBoatMotion();
    }

    void AnimateBoatMotion()
    {
        float time = Time.time;

        // Simulate wave up/down motion
        float waveY = Mathf.Sin(time * waveFrequency) * waveHeight;
        boatBody.localPosition = initialLocalPos + new Vector3(0f, waveY, 0f);

        // Simulate tilting left/right
        float tiltZ = Mathf.Sin(time * tiltSpeed) * tiltAngle;
        Quaternion tiltRot = Quaternion.Euler(0f, 0f, tiltZ);
        boatBody.localRotation = Quaternion.Slerp(boatBody.localRotation, initialLocalRot * tiltRot, Time.deltaTime * 3f);

        // Let passengers react using actual motion values
        if (passengers != null)
        {
            foreach (var p in passengers)
            {
                if (p != null)
                    p.ReactToBoatMotion(waveY, tiltZ); // ✅ Pass vertical bob and roll angle
            }
        }
    }
}