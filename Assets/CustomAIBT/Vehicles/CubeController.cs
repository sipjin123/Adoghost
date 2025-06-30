using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float moveSpeed = 5f;
 
     void Update()
     {
         // Get input axes
         float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
         float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down

         // Build movement vector
         Vector3 movement = new Vector3(horizontal, 0f, vertical) * moveSpeed * Time.deltaTime;

         // Apply movement
         transform.Translate(movement, Space.World); 
     }
}
