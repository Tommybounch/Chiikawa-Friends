using UnityEngine;

public class debugger : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)  // Checks if any key was pressed
        {
            Debug.Log("A key was pressed");
        }

        if (Input.GetKeyDown(KeyCode.I))  // Specific check for "I"
        {
            Debug.Log("I key pressed");
        }

        if (Input.GetKeyDown(KeyCode.Space))  // Specific check for Spacebar
        {
            Debug.Log("Spacebar pressed");
        }
    }
}
