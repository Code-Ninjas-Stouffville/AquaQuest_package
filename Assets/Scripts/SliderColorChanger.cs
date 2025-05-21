using UnityEngine;
using UnityEngine.UI;

public class SliderColorChanger : MonoBehaviour
{
    // Reference to the Slider component
    public Slider slider;

    // Reference to the Image component in the child object
    public Image fillImage;

    public float midThreshold = 1.5f;
    public float maxThreshold = 4f;

    // Optionally, define the color range for the slider
    public Color minColor = Color.red;
    public Color maxColor = Color.green;
    public Color midColor = Color.yellow;
    void Start()
    {
        // Ensure the Slider and Image are set
        if (slider == null || fillImage == null)
        {
            Debug.LogError("Slider or Image component is not assigned!");
            return;
        }

        // Initialize the fill color based on the current slider value
        UpdateFillColor(slider.value);

        // Add listener to the slider value change event
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // This function is called whenever the slider value changes
    void OnSliderValueChanged(float value)
    {
        // Update the fill color based on the new slider value
        UpdateFillColor(value);
    }

    // Update the color of the fill image based on the slider value
    void UpdateFillColor(float value)
    {
        Color newColor;

        if (value <= midThreshold)
        {
            // Normalize from 0 to midThreshold
            float t = Mathf.InverseLerp(0f, midThreshold, value);
            newColor = Color.Lerp(minColor, midColor, t);
        }
        else
        {
            // Normalize from midThreshold to maxThreshold
            float t = Mathf.InverseLerp(midThreshold, maxThreshold, value);
            newColor = Color.Lerp(midColor, maxColor, t);
        }

        fillImage.color = newColor;
    }

    // Optional: Clean up when this script is destroyed
    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}
