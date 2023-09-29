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

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class SplashScreenManager : MonoBehaviour
{

    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private GameObject flashObject = null;

    [Header("Logo Sprites")]
    [SerializeField] private Sprite defualtLogo = null;
    [SerializeField] private Sprite alternateLogo = null;

    [Header("Additional Settings")]
    [SerializeField] private float timeTillSwitchScene = 4.0f;
    [SerializeField] private float fadeTime = 0.25f;

    private bool switchedLogos = false;

    private float timeElapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

        // Add extra time to account for loading
        timeTillSwitchScene += 0.5f;

        // Set logo to default
        GetComponent<SpriteRenderer>().sprite = defualtLogo;

        // Set sprite opacity to zero
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);

    }

    // Update is called once per frame
    void Update()
    {

        // Time elapsed
        timeElapsed += Time.deltaTime;

        // To ensure game has properly loaded
        if (timeElapsed < 0.5f) return;

        // Fade in logo
        if (GetComponent<SpriteRenderer>().color.a < 1.0f)
        {
            GetComponent<SpriteRenderer>().color = new Color(
                255, 
                255, 
                255, 
                Mathf.Lerp(GetComponent<SpriteRenderer>().color.a, 1.0f, Time.deltaTime * fadeTime)
            );
        }

        // Change logo when halfway thru time
        if (!switchedLogos && timeElapsed >= timeTillSwitchScene / 2)
        {

            // Prevent from running again
            switchedLogos = true;

            // Screen flash
            flashObject.GetComponent<ScreenFlash>().enableFlash = true;

            // Set new logo
            GetComponent<SpriteRenderer>().sprite = alternateLogo;

            // Play knife sheeng sound
            GetComponent<AudioSource>().Play();

        }

        // Check if we should switch scenes
        if (timeElapsed >= timeTillSwitchScene)
        {
            SceneManager.LoadScene(mainMenuScene);
        }

    }
}
