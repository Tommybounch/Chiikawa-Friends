using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
     // This method will be called when the button is pressed
    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene"); // Name of the scene to load
    }

}
