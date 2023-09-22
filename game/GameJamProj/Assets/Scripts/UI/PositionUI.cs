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

    // Transform component
    Transform transform = null;

    // Start function
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        // Guard in case transform component cannot be found
        if (transform == null) return;

        // Calculate the size of 1 quadrant of the coordinate plane
        Vector2 quadrantSize = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

        // Position self according to config
        Vector3 finalPosition = new Vector3(0, 0, 0);

        // X position
        switch (positionX)
        {
            case -1:    // Align left
                finalPosition.x = -quadrantSize.x;
                break;
            case 1:     // Align right
                finalPosition.x = quadrantSize.x;
                break;
            default:    // Align center
                finalPosition.x = 0;
                break;
        }

        // Y position
        switch (positionY)
        {
            case -1:    // Align bottom
                finalPosition.y = -quadrantSize.y;
                break;
            case 1:     // Align top
                finalPosition.y = quadrantSize.y;
                break;
            default:    // Align center
                finalPosition.y = 0;
                break;
        }

        // Apply offset
        finalPosition += new Vector3(offsetX, offsetY, 0);

        // Set position to finalPosition
        transform.localPosition = finalPosition;
        Debug.Log(finalPosition);

    }
}
