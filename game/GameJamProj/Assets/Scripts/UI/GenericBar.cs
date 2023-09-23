/**
 * Author: Hudson
 * Contributors: 
 * Description: Controls health bar in UI
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
    [SerializeField] private float barMaxSize = 400.0f;
    [SerializeField] private float barChangeSpeed = 10.0f;

    [SerializeField] private bool enforceBarBounds = true;
    [SerializeField] private bool centeredBar = false;
    [SerializeField] private bool displayAsPercentage = false;

    /* Bar display info */
    public float maxBarValue = 100.0f;
    public float currentBarValue = 0.0f;

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

        // Guard against missing bar transform component
        if (m_barTransform == null) return;

        // Check if we should enforce bar bounds
        if (enforceBarBounds && currentBarValue > maxBarValue)
            currentBarValue = maxBarValue;
        if (enforceBarBounds && currentBarValue < 0)
            currentBarValue = 0;

        // Modify bar size
        m_barTransform.localScale = new Vector3(
            Mathf.Lerp(
                m_barTransform.localScale.x,
                (currentBarValue / maxBarValue) * barMaxSize,
                Time.deltaTime * barChangeSpeed
            ),
            m_barTransform.localScale.y,
            m_barTransform.localScale.z
        );

        // Modify offset position
        if(!centeredBar)
        {
            m_barTransform.localPosition =
                new Vector3(
                    (m_barTransform.localScale.x / 2) - (barMaxSize / 2),
                    m_barTransform.localPosition.y,
                    m_barTransform.localPosition.z
                );
        }

        // Guard against no bar text
        if (m_barText == null) return;

        /* ANYTHING BELOW HERE IS ONLY RUN IF BAR TEXT IS SPECIFIED */

        m_barText.text = (
            displayAsPercentage ? 
                "" + (currentBarValue / maxBarValue) * 100 + "%"
            :
                currentBarValue.ToString() + " / " + maxBarValue.ToString()
        );

    }

}
