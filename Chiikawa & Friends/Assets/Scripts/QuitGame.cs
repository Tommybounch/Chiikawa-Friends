using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
      // Method to quit the application
    public void QuitApplication()
    {
        // If the game is running in the editor, stop playing the scene
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); // Quit the game if it's a build
        #endif
    }
}
