/**
 * Author: Hudson
 * Contributors: 
 * Description: Easter egg sound effect
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class LogoEasterEgg : MonoBehaviour
{

    [SerializeField] private Sprite logoEasterEgg = null;
    [SerializeField] private bool openGithubRepo = false;

    private Sprite defaultLogo = null;

    // Start is called before the first frame update
    void Start()
    {
        defaultLogo = GetComponent<SpriteRenderer>().sprite;
    }

    void OnMouseDown()
    {
        if (openGithubRepo)
            Application.OpenURL("https://github.com/Cutie-Patooties/GameJam2023");
        else if (logoEasterEgg != null)
        {
            // Set sprite to easter egg logo
            GetComponent<SpriteRenderer>().sprite = logoEasterEgg;
            // Play knife sheeng sound
            GetComponent<AudioSource>().Play();
        }
    }

}
