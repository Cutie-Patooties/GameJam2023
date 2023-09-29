/**
 * Author: Hudson
 * Contributors: 
 * Description: Defines the shotgun ranged weapon
**/

using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour
{

    [SerializeField] private string mainMenuScene = "MainMenu";

    [Header("Logo Sprites")]
    [SerializeField] private Sprite defualtLogo = null;
    [SerializeField] private Sprite alternateLogo = null;

    [Header("Additional Settings")]
    [SerializeField] private float timeTillSwitchScene = 4.0f;

    private float timeElapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = defualtLogo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
