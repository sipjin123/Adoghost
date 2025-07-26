using UnityEngine;

public class AgentManipulator : MonoBehaviour
{
    public float RangeFactor = 2;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.LogError(gameObject.name + "::Debugging Range Factor: " + RangeFactor);
            DebugDraw.Sphere(transform.position, RangeFactor, Color.red, 5);
        }
    }
}
