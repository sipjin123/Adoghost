using UnityEngine;

public static class DebugDraw
{
    public static void Sphere(Vector3 position, float radius, Color color, float duration = 0f, int segments = 20)
    {
        for (int i = 0; i < segments; i++)
        {
            float theta1 = (i / (float)segments) * 2 * Mathf.PI;
            float theta2 = ((i + 1) / (float)segments) * 2 * Mathf.PI;

            Vector3 offset1 = new Vector3(Mathf.Cos(theta1), 0, Mathf.Sin(theta1)) * radius;
            Vector3 offset2 = new Vector3(Mathf.Cos(theta2), 0, Mathf.Sin(theta2)) * radius;
            Debug.DrawLine(position + offset1, position + offset2, color, duration);

            offset1 = new Vector3(0, Mathf.Cos(theta1), Mathf.Sin(theta1)) * radius;
            offset2 = new Vector3(0, Mathf.Cos(theta2), Mathf.Sin(theta2)) * radius;
            Debug.DrawLine(position + offset1, position + offset2, color, duration);

            offset1 = new Vector3(Mathf.Cos(theta1), Mathf.Sin(theta1), 0) * radius;
            offset2 = new Vector3(Mathf.Cos(theta2), Mathf.Sin(theta2), 0) * radius;
            Debug.DrawLine(position + offset1, position + offset2, color, duration);
        }
    }
}