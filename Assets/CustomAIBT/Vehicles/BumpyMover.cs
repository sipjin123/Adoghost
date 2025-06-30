using UnityEngine;

public class BumpyMover : MonoBehaviour
{
 [Header("Waypoint Movement")]
    public Transform[] waypoints;
    public float speed = 5f;
    public float reachThreshold = 0.5f;

    [Header("Bumpy Behavior")]
    public Transform body; // The child object
    public float bumpAmplitude = 0.3f;
    public float bumpFrequency = 2f;

    [Header("Body Tilt")]
    public float tiltAmplitude = 10f;     // Degrees
    public float tiltFrequency = 2f;
    public float tiltSmoothness = 5f;

    private int index = 0;
    private Vector3 bodyInitialLocalPos;
    private Quaternion bodyInitialLocalRot;

    private PassengerReactor[] passengers;

    void Start()
    {
        if (body != null)
        {
            bodyInitialLocalPos = body.localPosition;
            bodyInitialLocalRot = body.localRotation;
            
            // Find all passenger scripts in children
            passengers = body.GetComponentsInChildren<PassengerReactor>();
        }
    }

    void Update()
    {
        if (waypoints.Length == 0 || body == null) return;

        // Move parent toward waypoint
        Vector3 target = waypoints[index].position;
        Vector3 moveDir = (target - transform.position).normalized;
        transform.position += moveDir * speed * Time.deltaTime;

        // Advance to next waypoint
        if (Vector3.Distance(transform.position, target) < reachThreshold)
            index = (index + 1) % waypoints.Length;

        // === Vertical Bump ===
        float verticalNoise = Mathf.PerlinNoise(Time.time * bumpFrequency, 0f);
        float yOffset = (verticalNoise - 0.5f) * 2f * bumpAmplitude;
        body.localPosition = bodyInitialLocalPos + new Vector3(0, yOffset, 0);

        // === Left-Right Tilt ===
        float lateralNoise = Mathf.PerlinNoise(Time.time * tiltFrequency, 5f);
        float tiltAngle = (lateralNoise - 0.5f) * 2f * tiltAmplitude;

        Quaternion targetTilt = bodyInitialLocalRot * Quaternion.Euler(0f, 0f, tiltAngle); // roll on Z-axis
        body.localRotation = Quaternion.Slerp(body.localRotation, targetTilt, Time.deltaTime * tiltSmoothness);
        
        float time = Time.time;
        // === Call Passenger Reactions ===
        foreach (var passenger in passengers)
        {
            if (passenger != null)
                passenger.ReactToBump(time);
        }

        FaceDirection(moveDir);
    }

    void FaceDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}