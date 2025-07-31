using UnityEngine;
using UnityEngine.UI;

public class LightingEditorPanel : MonoBehaviour
{
    [Header("UI Elements")]
    public Toggle fogToggle;
    public InputField fogStartInput;
    public InputField fogEndInput;
    
    [Header("Fog")]
    public bool fogEnabled = false;
    public float fogStart = 0f;
    public float fogEnd = 100f;

    private void Start()
    {
        RenderSettings.fogMode = FogMode.Linear;
        
        fogToggle.isOn = fogEnabled;
        fogStartInput.text = fogStart.ToString();
        fogEndInput.text = fogEnd.ToString();
        
        fogStartInput.interactable = fogEnabled;
        fogEndInput.interactable = fogEnabled;

        fogToggle.onValueChanged.AddListener(OnFogToggleChanged);
        fogStartInput.onValueChanged.AddListener(OnFogStartInputChanged);
        fogEndInput.onValueChanged.AddListener(OnFogEndInputChanged);
        
        ApplyFogSettings();
    }

    private void OnFogToggleChanged(bool isOn)
    {
        fogEnabled = isOn;
        fogStartInput.interactable = isOn;
        fogEndInput.interactable = isOn;
        ApplyFogSettings();
    }

    private void OnFogStartInputChanged(string value)
    {
        if (float.TryParse(value, out float newFogStart))
        {
            fogStart = newFogStart;
            ApplyFogSettings();
        }
    }

    private void OnFogEndInputChanged(string value)
    {
        if (float.TryParse(value, out float newFogEnd))
        {
            fogEnd = newFogEnd;
            ApplyFogSettings();
        }
    }

    private void ApplyFogSettings()
    {
        RenderSettings.fog = fogEnabled;
        RenderSettings.fogStartDistance = fogStart;
        RenderSettings.fogEndDistance = fogEnd;
    }

    private void Update()
    {
        if (fogStartInput.text != fogStart.ToString())
        {
            fogStartInput.text = fogStart.ToString();
        }
        if (fogEndInput.text != fogEnd.ToString())
        {
            fogEndInput.text = fogEnd.ToString();
        }
    }

    private void OnDestroy()
    {
        fogToggle.onValueChanged.RemoveListener(OnFogToggleChanged);
        fogStartInput.onValueChanged.RemoveListener(OnFogStartInputChanged);
        fogEndInput.onValueChanged.RemoveListener(OnFogEndInputChanged);
    }
}