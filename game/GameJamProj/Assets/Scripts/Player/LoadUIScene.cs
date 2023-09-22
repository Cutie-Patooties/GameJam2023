/**
 * Author: Hudson
 * Contributors: 
 * Description: Loads the UI scene ontop of the current scene
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadUIScene : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // Load the scene
        SceneManager.LoadScene("PlayerUI", LoadSceneMode.Additive);
    }

}
