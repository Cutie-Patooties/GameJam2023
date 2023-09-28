/**
 * Author: Hudson
 * Contributors: 
 * Description: for making a generic bar
**/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenericBar : MonoBehaviour
{

    /* Bar and text GameObjects */
    // Required
    [SerializeField] private GameObject barObject = null;
    // Optional
    [SerializeField] private GameObject textObject = null;

    /* Control bar attributes */
    [Header("Bar Attributes")]
    [SerializeField] private float barMaxSize = 400.0f;
    [Tooltip("How quick to animate the change speed (set to anything less than 0 to disable)")]
    [SerializeField] private float barChangeSpeed = 10.0f;

    /* Bar display info */
    [Header("Default Bar Values")]
    [SerializeField] private float maxBarValue = 100.0f;
    [SerializeField] private float currentBarValue = 0.0f;

    /* More options */
    [Header("Additional Options")]
    [SerializeField] private bool enforceBarBounds = true;
    [SerializeField] private bool enforceTextBounds = false;
    [SerializeField] private bool centeredBar = false;
    [SerializeField] private bool displayAsPercentage = false;  // overwrites formatAsTime
    [SerializeField] private bool formatAsTime = false;

    // Store transform component of the bar
    private Transform m_barTransform = null;

    // Store TextMeshProUGUI component for the bar text
    private TextMeshProUGUI m_barText = null;

    // Start is called before the first frame update
    void Start()
    {
        
        // Attempt to grab transform component on bar
        if(barObject != null)
            m_barTransform = barObject.GetComponent<Transform>();

        // Attempt to grab text component on bar text
        if(textObject != null)
            m_barText = textObject.GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {

        // Modify bar size
        UpdateBar();

        // Update bar text
        UpdateText();

    }

    /// <summary>
    /// Sets both current and max values for the bar
    /// </summary>
    /// <param name="newValue">Update the old current value with a new one</param>
    /// <param name="newMaxValue">Update the old max value with a new one</param>
    public void SetBarValue(float newValue, float newMaxValue)
    {
        currentBarValue = newValue;
        maxBarValue = newMaxValue;
    }

    /// <summary>
    /// Sets only the current value for the bar and leaves the old max the same
    /// </summary>
    /// <param name="newValue">Update the current value with a new one</param>
    public void SetBarValue(float newValue)
    {
        SetBarValue(newValue, maxBarValue);
    }

    private void UpdateBar()
    {

        // Check if we should enforce bar bounds
        if (enforceBarBounds && currentBarValue > maxBarValue)
            currentBarValue = maxBarValue;
        if (enforceBarBounds && currentBarValue < 0)
            currentBarValue = 0;

        // Guard against missing bar transform component
        if (m_barTransform == null) return;

        // Modify bar size
        m_barTransform.localScale = new Vector3(
            (barChangeSpeed > 0.0f ? 
                Mathf.Lerp(
                    m_barTransform.localScale.x,
                    (currentBarValue / maxBarValue) * barMaxSize,
                    Time.deltaTime * barChangeSpeed
                ) :
                (currentBarValue / maxBarValue) * barMaxSize
            ),
            m_barTransform.localScale.y,
            m_barTransform.localScale.z
        );

        // Modify offset position
        if (!centeredBar)
        {
            m_barTransform.localPosition =
                new Vector3(
                    (m_barTransform.localScale.x / 2) - (barMaxSize / 2),
                    m_barTransform.localPosition.y,
                    m_barTransform.localPosition.z
                );
        }

    }

    private void UpdateText()
    {

        // Check if we should enforce text bounds
        if (enforceTextBounds && currentBarValue > maxBarValue)
            currentBarValue = maxBarValue;
        if (enforceTextBounds && currentBarValue < 0)
            currentBarValue = 0;

        // Guard against no bar text
        if (m_barText == null) return;

        // Display bar
        if(displayAsPercentage)
        {
            m_barText.text = Mathf.Round((currentBarValue / maxBarValue) * 100).ToString() + "%";
        }
        else if(formatAsTime)
        {
            m_barText.text = 
                ((int)maxBarValue / 60 != 0 ? ((int)currentBarValue / 60).ToString("00") + ":" : "") + 
                ((int)currentBarValue % 60).ToString("00") + 
                (currentBarValue % 1).ToString(".000") + 
                ((int)maxBarValue / 60 == 0 ? " sec(s)" : "");
        }
        else m_barText.text = currentBarValue.ToString() + " / " + maxBarValue.ToString();

    }

}