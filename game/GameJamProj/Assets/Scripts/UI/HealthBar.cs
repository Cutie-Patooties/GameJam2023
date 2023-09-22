/**
 * Author: Hudson
 * Contributors: 
 * Description: Controls health bar in UI
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private float healthBarMaxSize = 3.0f;
    [SerializeField] private float healthbarSpeed = 10.0f;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // Get the player controller from the player object
        playerController = GameObject.Find("EntityPlayer").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        // Guard against crashes due to no player
        if (playerController == null) return;

        // Update the current max health
        float maxHealth = playerController.maxHealth;
        float currHealth = playerController.currentHealth;

        Transform barTransform = GetComponent<Transform>();

        barTransform.localScale = new Vector3(
            Mathf.Lerp(
                barTransform.localScale.x,
                (currHealth / maxHealth) * healthBarMaxSize,
                Time.deltaTime * healthbarSpeed
            ), 
            barTransform.localScale.y, 
            barTransform.localScale.z
        );

    }
}
