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

    [SerializeField] string uiSceneName = "UIScene";

    // Start is called before the first frame update
    void Start()
    {
        // Load the scene
        SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
