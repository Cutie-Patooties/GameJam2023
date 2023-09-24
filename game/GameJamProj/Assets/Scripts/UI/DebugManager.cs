/**
 * Author: Hudson
 * Contributors: 
 * Description: Controls health bar in UI
**/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DebugManager : MonoBehaviour
{

    // Game Info
    [SerializeField] private string gameTitle = "Game Title";
    [SerializeField] private string gameVersion = "0.0.0";

    // UI Canvas object
    [SerializeField] private GameObject uiCanvas = null;

    // Canvas component
    private Canvas uiCanvasComponent = null;

    // Stores additional debug fields
    private SortedDictionary<string, string> customDebugFields = null;

    // For CalcFPS function
    private int fps = 0;
    private float frameTime = 0.0f;

    private int CalcFPS_frameCount = 0;
    private float CalcFPS_dtTotal = 0.0f;
    private float CalcFPS_updateRate = 2.0f;    // Updates x times per second

    // Awake is called before start
    private void Awake()
    {
        // Instantiate the sorted dictionary of custom debug fields
        customDebugFields = new SortedDictionary<string, string>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set uiCanvasComponent to the UI canvas component
        if(uiCanvas != null)
            uiCanvasComponent = uiCanvas.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        CalcFPS();
        UpdateDebugInfo();
    }

    /// <summary>
    /// Modifies custom debug field with given text. If fieldName is not found, creates a new one with that info
    /// </summary>
    /// <param name="fieldName">The key</param>
    /// <param name="content">The content you want to display in the debug text info</param>
    public void UpdateCustomDebugField(string fieldName, string content)
    {
        // Add or set field
        customDebugFields[fieldName] = content;
    }

    private void UpdateDebugInfo()
    {

        // Reset textbox
        GetComponent<TextMeshProUGUI>().text = "";

        // Set game title and version
        PrintToDebugln(gameTitle + " (v" + gameVersion + ")");

        // Set FPS counter
        PrintToDebugln(fps.ToString() + " fps (" + frameTime + " ms)\n");

        // Print additional info
        foreach(var debugText in customDebugFields)
            PrintToDebugln(debugText.Value);

    }

    private void CalcFPS()
    {

        ++CalcFPS_frameCount;
        CalcFPS_dtTotal += Time.deltaTime;

        if(CalcFPS_dtTotal > 1.0f / CalcFPS_updateRate)
        {
            frameTime = Time.deltaTime * 1000.0f;
            fps = (int)(CalcFPS_frameCount / CalcFPS_dtTotal);
            CalcFPS_frameCount = 0;
            CalcFPS_dtTotal -= 1.0f / CalcFPS_updateRate;
        }
    }

    private void PrintToDebug(string text)
    {
        GetComponent<TextMeshProUGUI>().text += text;
    }

    private void PrintToDebugln(string text = "")
    {
        PrintToDebug(text + "\n");
    }

}
