/**
 * Author: Hudson
 * Contributors: 
 * Description: Controls health bar in UI
**/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Search;

public class HealthBar : MonoBehaviour
{

    // The healthbar and text objects to modify
    [SerializeField] private GameObject healthBarObject = null;
    [SerializeField] private GameObject healthTextObject = null;

    [SerializeField] private float healthBarMaxSize = 400.0f;
    [SerializeField] private float healthbarSpeed = 10.0f;

    [SerializeField] private bool centeredBar = true;

    // Gets player controller script
    private PlayerController playerController = null;

    // Store transform component for health bar
    Transform barTransform = null;

    // Store TextMeshProUGUI component for health bar text
    TextMeshProUGUI barText = null;

    // Start is called before the first frame update
    void Start()
    {
        // Get the player controller from the player object
        playerController = GameObject.Find("EntityPlayer").GetComponent<PlayerController>();
        // Set the barTransform to this transform component
        if(healthBarObject != null)
            barTransform = healthBarObject.GetComponent<Transform>();
        // Gets parent text mesh
        if(healthTextObject != null)
            barText = healthTextObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        // Guard against crashes due to no player
        if (playerController == null) return;

        // Update the current max health
        float maxHealth = playerController.maxHealth;
        float currHealth = playerController.currentHealth;

        // Ensure parent GameObject is a text box (if not assume this is intentional and no text is wanted)
        if(barText != null)
        {
            barText.text = "HP  " + currHealth.ToString() + " / " + maxHealth.ToString();
        }

        // Guards against missing bar transform
        if (barTransform == null) return;

        // Ensure that HP bar stays within bounds
        if (currHealth < 0.0f) currHealth = 0.0f;
        if (currHealth > maxHealth) currHealth = maxHealth;

        // Set the scale of the health bar
        barTransform.localScale = new Vector3(
            Mathf.Lerp(
                barTransform.localScale.x,
                (currHealth / maxHealth) * healthBarMaxSize,
                Time.deltaTime * healthbarSpeed
            ), 
            barTransform.localScale.y, 
            barTransform.localScale.z
        );

        // Offset position by scale (if not a centered bar)
        if (!centeredBar)
        {
            barTransform.localPosition =
                new Vector3(
                    (barTransform.localScale.x / 2.0f) - (healthBarMaxSize / 2.0f),
                    barTransform.localPosition.y,
                    barTransform.localPosition.z
                );
        }

    }
}
