/**
 * Author: Hudson
 * Contributors: 
 * Description: Defines the shotgun ranged weapon
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class ButtonScript : MonoBehaviour
{

    [Header("Button Sprite Settings")]
    [SerializeField] private Sprite buttonSprite = null;
    [SerializeField] private Sprite buttonSpriteHover = null;

    [Header("Scene Switch Settings")]
    [SerializeField] private bool enableSwitchScene = false;
    [SerializeField] private string sceneName = "";

    [Header("Quit Button")]
    [SerializeField] private bool enableQuitGame = false;

    // Start is called before the first frame update
    void Start()
    {
        if (buttonSprite == null)
            buttonSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void OnMouseDown()
    {

        // Check if we want to switch scene
        if (enableSwitchScene)
            SceneManager.LoadScene(sceneName);

        // Check if we want to quit the game
        else if (enableQuitGame)
            Application.Quit();

    }

    private void OnMouseEnter()
    {
        // Change to hover sprite
        if(buttonSpriteHover != null)
            GetComponent<SpriteRenderer>().sprite = buttonSpriteHover;
    }

    private void OnMouseExit()
    {
        // Change to default sprite
        GetComponent<SpriteRenderer>().sprite = buttonSprite;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(sceneName);
    }

}
