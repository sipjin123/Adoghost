using UnityEngine;
using System;

public class VisionDetectorComp : MonoBehaviour
{
 
    public static event Action<GameObject> OnGameObjectBroadcast;
    public float viewDistance = 10f;
    public float viewAngle = 60f; // Total angle (degrees)
    public int rayCount = 20;     // Number of rays in the fan
    public LayerMask targetLayer;

    
    public void ScanForTargets()
    {
        float halfAngle = viewAngle / 2f;
        float angleStep = viewAngle / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -halfAngle + (angleStep * i);
            Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);
            Vector3 direction = rotation * transform.forward;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, viewDistance, ~0))
            {
                if (hit.collider.CompareTag("Targets"))
                {
                    ICanBeKilled killable = hit.collider.gameObject.GetComponent<ICanBeKilled>();
                    if (killable == null || !killable.CanBeKilled())
                        continue;
                    OnTargetSeen(hit.collider.gameObject);
                    Debug.DrawRay(transform.position, direction * hit.distance, Color.green);
                }
                else
                {
                    Debug.DrawRay(transform.position, direction * hit.distance, Color.red);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, direction * viewDistance, Color.gray);
            }
        }
    }
    
    void OnTargetSeen(GameObject target)
    {
        OnGameObjectBroadcast?.Invoke(target);
    }
}