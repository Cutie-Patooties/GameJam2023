/**
 * Author: Hudson
 * Contributors: 
 * Description: A script to ensure that the UI is properly positioned regardless of end user's screen size or aspect ratio
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    // Control screen alignment
    [Tooltip("Determines horizontal alignment (-1 is left, 0 is center, 1 is right)")]
    [SerializeField] [Range(-1, 1)] private int positionX = 0;
    [Tooltip("Determines vertical alignment (-1 is bottom, 0 is center, 1 is top)")]
    [SerializeField] [Range(-1, 1)] private int positionY = 0;

    // Apply offset if necessary
    [SerializeField] private float offsetX = 0.0f;
    [SerializeField] private float offsetY = 0.0f;

    // Camera in UI Level
    private Camera uiCam = null;

    // Start is called before the first frame update
    void Start()
    {
        // Set uiCam to camera in scene
        uiCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        // Don't do anything if we cannot get the camera
        if (uiCam == null) return;

        // Get the width and height of the screen
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        // Convert this data for use in world coordinates
        Vector3 viewableSize = uiCam.ScreenToWorldPoint(new Vector3(screenSize.x, screenSize.y, 10));

        Debug.Log("Screen size: (" + screenSize.x + ", " + screenSize.y + ")");
        Debug.Log("Viewable size: (" + viewableSize.x + ", " + viewableSize.y + ")");

    }
}
