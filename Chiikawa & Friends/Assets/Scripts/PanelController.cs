using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
            // Check if the click is on a UI element
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // Get the RectTransform of the panel
                RectTransform rectTransform = panel.GetComponent<RectTransform>();

                if (rectTransform != null)
                {
                    // Convert the mouse position to local coordinates relative to the panel
                    Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);

                    // Check if the local mouse position is within the panel's rect
                    if (!rectTransform.rect.Contains(localMousePosition))
                    {
                        HidePanel(); // Hide the panel if the click is outside
                    }
                }
            }
        }
    }
}