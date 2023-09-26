/**
 * Author: Hudson
 * Contributors: 
 * Description: Makes the camera floaty for visual effect and because i wanted to get at least something done today
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCamera : MonoBehaviour
{

    [SerializeField] private float divideAmount = 12.0f;
    [SerializeField] private float floatStrength = 10.0f;

    // Update is called once per frame
    void Update()
    {

        // Get mouse position
        Vector3 mousePos = Input.mousePosition;

        // Get cam coords in world space
        Vector3 cameraWorld = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -10.0f));

        transform.localPosition = new Vector3(
            Mathf.Lerp(transform.localPosition.x, cameraWorld.x / divideAmount, Time.deltaTime * floatStrength),
            Mathf.Lerp(transform.localPosition.y, cameraWorld.y / divideAmount, Time.deltaTime * floatStrength),
            -10.0f
        );


    }
}
