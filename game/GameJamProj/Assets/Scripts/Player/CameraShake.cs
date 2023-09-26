/**
 * Author: Alan
 * Contributors: Hudson
 * Description: This script performs a screen shake effect for hitting an enemy
**/

using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Variables needed to shake the camera
    private Transform cameraTransform;
    private float shakeDuration;
    private float shakeMagnitude;
    private readonly float dampingSpeed = 1.0f;
    private Vector3 initialPosition;

    private bool disableControlWhenUnused = false;

    // This script will shake the player's camera
    private void Awake()
    {
        initialPosition.z = -10;
        cameraTransform = Camera.main.transform;
    }

    // This will shake the camera over a period of time
    private void Update()
    {
        if (shakeDuration > 0)
        {
            disableControlWhenUnused = false;
            cameraTransform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else if(!disableControlWhenUnused)
        {
            disableControlWhenUnused = true;
            shakeDuration = 0f;
            cameraTransform.localPosition = initialPosition;
        }
    }

    // This function determines how exactly the camera will shake
    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        initialPosition = cameraTransform.localPosition;
    }
}