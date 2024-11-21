using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadHomeScene : MonoBehaviour
{
   // Function to load the "HomeScreen" scene
    public void LoadHomeScreen()
    {
        SceneManager.LoadScene("HomeScreen"); // Loads the HomeScreen scene
    }
}
