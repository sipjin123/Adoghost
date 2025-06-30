using UnityEngine;

public class BumpyVehicleController : MonoBehaviour
{
    [Header("Waypoint Movement")] public Transform[] waypoints;
    public float moveSpeed = 5f;
    public float reachThreshold = 0.5f;
    private int currentWaypoint = 0;

    [Header("Bumpy Wheels")] public Transform wheelLeft;
    public Transform wheelRight;
    public Transform body;

    public float bumpAmplitude = 0.3f;
    public float bumpFrequency = 2f;
    public float wheelSeparation = 2f; // for visual spacing

    [Header("Body Suspension")] public float bodySmoothSpeed = 5f;

    private Vector3 wheelLeftBasePos;
    private Vector3 wheelRightBasePos;
    private Vector3 bodyBaseLocalPos;

    void Start()
    {
        if (!wheelLeft || !wheelRight || !body)
        {
            Debug.LogWarning("BumpyVehicleController: Missing wheel or body references.");
            enabled = false;
            return;
        }

        wheelLeftBasePos = wheelLeft.localPosition;
        wheelRightBasePos = wheelRight.localPosition;
        bodyBaseLocalPos = body.localPosition;
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        // === Movement toward waypoint ===
        Vector3 target = waypoints[currentWaypoint].position;
        Vector3 moveDir = (target - transform.position).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // Rotate to face movement
        FaceDirection(moveDir);

        if (Vector3.Distance(transform.position, target) < reachThreshold)
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;

        // === Simulate wheel bounce ===
        float time = Time.time * bumpFrequency;
        float leftNoise = Mathf.PerlinNoise(time, 0f);
        float rightNoise = Mathf.PerlinNoise(time, 1f);

        float leftOffsetY = (leftNoise - 0.5f) * 2f * bumpAmplitude;
        float rightOffsetY = (rightNoise - 0.5f) * 2f * bumpAmplitude;

        Vector3 leftPos = wheelLeftBasePos + new Vector3(-wheelSeparation / 2f, leftOffsetY, 0);
        Vector3 rightPos = wheelRightBasePos + new Vector3(wheelSeparation / 2f, rightOffsetY, 0);

        wheelLeft.localPosition = leftPos;
        wheelRight.localPosition = rightPos;

        // === Smooth body between the two wheels ===
        Vector3 averagePos = (wheelLeft.position + wheelRight.position) / 2f;
        Vector3 localBodyTarget = transform.InverseTransformPoint(averagePos);
        Vector3 smoothed = Vector3.Lerp(body.localPosition, localBodyTarget, Time.deltaTime * bodySmoothSpeed);
        body.localPosition = new Vector3(bodyBaseLocalPos.x, smoothed.y, bodyBaseLocalPos.z);
    }

    void FaceDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
    }
}