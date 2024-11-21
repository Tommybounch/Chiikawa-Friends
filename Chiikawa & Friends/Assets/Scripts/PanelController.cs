using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelController : MonoBehaviour
{
     public GameObject panel; // Reference to the panel GameObject

    // Show the panel
    public void ShowPanel()
    {
        panel.SetActive(true);  // Makes the panel visible
    }

    // Hide the panel
    public void HidePanel()
    {
        panel.SetActive(false); // Hides the panel
    }

    // Detect click outside the panel to close it
    void Update()
    {
        if (panel.activeSelf && Input.GetMouseButtonDown(0)) // Left-click detection
        {
            // Check if the click is outside the panel
            if (!RectTransformUtility.RectangleContainsScreenPoint(panel.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
            {
                // Also check if the click is on a UI element (like a button)
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    HidePanel(); // Hide the panel if click is outside and not on a UI element
                }
            }
        }
    }
}