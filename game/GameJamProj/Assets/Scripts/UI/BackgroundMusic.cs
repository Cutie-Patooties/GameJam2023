/**
 * Author: Hudson
 * Contributors: 
 * Description: Manages the background music
**/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{

    [SerializeField] private AudioClip audioClip = null;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().clip = audioClip;
        GetComponent<AudioSource>().Play();
    }

}
