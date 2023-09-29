/**
 * Author: Hudson
 * Contributors: 
 * Description: Defines the shotgun ranged weapon
**/

using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{

    [SerializeField] private string mainMenuScene = "MainMenu";

    [Header("Logo Sprites")]
    [SerializeField] private Sprite defualtLogo = null;
    [SerializeField] private Sprite alternateLogo = null;

    [Header("Additional Settings")]
    [SerializeField] private float timeTillSwitchScene = 4.0f;

    private bool switchedLogos = false;

    private float timeElapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = defualtLogo;
    }

    // Update is called once per frame
    void Update()
    {

        // Time elapsed
        timeElapsed += Time.deltaTime;

        // Change logo when halfway thru time
        if (!switchedLogos && timeElapsed >= timeTillSwitchScene / 2)
        {

            // Prevent from running again
            switchedLogos = true;

            // Set new logo
            GetComponent<SpriteRenderer>().sprite = alternateLogo;

            // Play knife sheeng sound
            GetComponent<AudioSource>().Play();

        }
        else if (timeElapsed >= timeTillSwitchScene)
        {
            SceneManager.LoadScene(mainMenuScene);
        }

    }
}
