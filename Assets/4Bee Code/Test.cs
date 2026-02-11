using UnityEngine;
using UnityEngine.InputSystem; // <--- NEW LINE REQUIRED

public class GlobalClickTest : MonoBehaviour
{
    void Update()
    {
        // OLD WAY: if (Input.GetMouseButtonDown(0))

        // NEW WAY:
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("GLOBAL CLICK DETECTED!");
        }
    }
}