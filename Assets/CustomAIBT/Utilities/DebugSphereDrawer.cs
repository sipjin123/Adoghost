using UnityEngine;

public class DebugSphereDrawer : MonoBehaviour
{
    public float radius = 2f;
    public Color color = Color.red;
    public float duration = 1f;

    void Update()
    {
        DrawDebugSphere(transform.position, radius, color, duration);
    }

    void DrawDebugSphere(Vector3 position, float radius, Color color, float duration)
    {
        int segments = 20;

        // Draw horizontal circle
        for (int i = 0; i < segments; i++)
        {
            float theta1 = (i / (float)segments) * 2 * Mathf.PI;
            float theta2 = ((i + 1) / (float)segments) * 2 * Mathf.PI;

            Vector3 pos1 = position + new Vector3(Mathf.Cos(theta1), 0, Mathf.Sin(theta1)) * radius;
            Vector3 pos2 = position + new Vector3(Mathf.Cos(theta2), 0, Mathf.Sin(theta2)) * radius;
            Debug.DrawLine(pos1, pos2, color, duration);
        }

        // Draw vertical circles (XZ plane and YZ plane)
        for (int i = 0; i < segments; i++)
        {
            float theta1 = (i / (float)segments) * 2 * Mathf.PI;
            float theta2 = ((i + 1) / (float)segments) * 2 * Mathf.PI;

            // XZ vertical circle
            Vector3 p1 = position + new Vector3(0, Mathf.Cos(theta1), Mathf.Sin(theta1)) * radius;
            Vector3 p2 = position + new Vector3(0, Mathf.Cos(theta2), Mathf.Sin(theta2)) * radius;
            Debug.DrawLine(p1, p2, color, duration);

            // YZ vertical circle
            Vector3 q1 = position + new Vector3(Mathf.Cos(theta1), Mathf.Sin(theta1), 0) * radius;
            Vector3 q2 = position + new Vector3(Mathf.Cos(theta2), Mathf.Sin(theta2), 0) * radius;
            Debug.DrawLine(q1, q2, color, duration);
        }
    }
}
