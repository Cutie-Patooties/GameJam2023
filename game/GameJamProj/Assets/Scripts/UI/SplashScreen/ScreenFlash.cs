/**
 * Author: Hudson
 * Contributors: 
 * Description: Defines the shotgun ranged weapon
**/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ScreenFlash : MonoBehaviour
{

    public bool enableFlash = false;

    [SerializeField] private float flashTime = 50.0f;

    private bool startedFlash = false;

    private float currTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Set sprite opacity to zero
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (!enableFlash) return;

        if (!startedFlash)
        {
            startedFlash = true;
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1.0f);
        }

        currTime += Time.deltaTime;

        GetComponent<SpriteRenderer>().color = new Color(
            255, 
            0,
            0,
            Mathf.Lerp(GetComponent<SpriteRenderer>().color.a, 0.0f, Time.deltaTime * flashTime)
        );

    }

}
