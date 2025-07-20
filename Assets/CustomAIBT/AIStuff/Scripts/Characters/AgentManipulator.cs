using UnityEngine;

public class AgentManipulator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.LogError("ASASA");
            DebugDraw.Sphere(transform.position, 5, Color.red, 5);
        }
    }
}
