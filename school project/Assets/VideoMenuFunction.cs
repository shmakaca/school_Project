using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class VideoSettings : MonoBehaviour
{
    // UI references
    public TMP_Dropdown displayModeDropdown;
    public TMP_Dropdown nvidiaReflexDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Slider maxFrameRateSlider;
    public Slider gammaSlider;
    public Slider contrastSlider;
    public Slider brightnessSlider;

    // On/Off Button Controller reference for VSync
    public OnOffButtonController vsyncButtonController;

    // Shader properties
    public Material postProcessMaterial;  // Assign this in the editor

    // Parent GameObject of the resolutionDropdown
    public GameObject resolutionParent;

    private void Start()
    {
        // Initialize settings from saved values or defaults
        InitializeSettings();

        // Assign listeners to UI elements
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        maxFrameRateSlider.onValueChanged.AddListener(value => SetMaxFrameRate((int)value));
        gammaSlider.onValueChanged.AddListener(SetGamma);
        contrastSlider.onValueChanged.AddListener(SetContrast);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        displayModeDropdown.onValueChanged.AddListener(SetDisplayMode);
        nvidiaReflexDropdown.onValueChanged.AddListener(SetNvidiaReflex);

        // Add a listener to the VSync button
        vsyncButtonController.onOffButton.onClick.AddListener(() => SetVSync(vsyncButtonController.isOn));

        // For testing in the editor
#if UNITY_EDITOR
        UpdateResolutionDropdown();
#endif
    }

    private void InitializeSettings()
    {
        // Load and apply saved settings
        SetResolution(PlayerPrefs.GetInt("Resolution", 0));
        SetMaxFrameRate(PlayerPrefs.GetInt("MaxFrameRate", (int)maxFrameRateSlider.value));
        SetGamma(PlayerPrefs.GetFloat("Gamma", gammaSlider.value));
        SetContrast(PlayerPrefs.GetFloat("Contrast", contrastSlider.value));
        SetBrightness(PlayerPrefs.GetFloat("Brightness", brightnessSlider.value));
        SetVSync(PlayerPrefs.GetInt("VSync", vsyncButtonController.isOn ? 1 : 0) == 1);
        SetDisplayMode(PlayerPrefs.GetInt("DisplayMode", displayModeDropdown.value));
        SetNvidiaReflex(PlayerPrefs.GetInt("NvidiaReflex", nvidiaReflexDropdown.value));
    }

    public void SetResolution(int value)
    {
        Resolution[] resolutions = Screen.resolutions;
        if (value >= 0 && value < resolutions.Length)
        {
            Resolution resolution = resolutions[value];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt("Resolution", value);
        }
    }

    public void SetMaxFrameRate(int value)
    {
        Application.targetFrameRate = value;
        PlayerPrefs.SetInt("MaxFrameRate", value);
    }

    public void SetGamma(float value)
    {
        if (postProcessMaterial != null)
        {
            postProcessMaterial.SetFloat("_Gamma", value);
            PlayerPrefs.SetFloat("Gamma", value);
        }
    }

    public void SetContrast(float value)
    {
        if (postProcessMaterial != null)
        {
            postProcessMaterial.SetFloat("_Contrast", value);
            PlayerPrefs.SetFloat("Contrast", value);
        }
    }

    public void SetBrightness(float value)
    {
        if (postProcessMaterial != null)
        {
            postProcessMaterial.SetFloat("_Brightness", value);
            PlayerPrefs.SetFloat("Brightness", value);
        }
    }

    public void SetVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSync", isOn ? 1 : 0);
        Debug.Log("VSync is set to " + (isOn ? "ON" : "OFF"));
    }

    public void SetDisplayMode(int value)
    {
        FullScreenMode mode = FullScreenMode.Windowed;
        switch (value)
        {
            case 0:
                mode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                mode = FullScreenMode.Windowed;
                break;
            case 2:
                mode = FullScreenMode.FullScreenWindow;
                break;
        }
        Screen.fullScreenMode = mode;
        PlayerPrefs.SetInt("DisplayMode", value);
        UpdateResolutionDropdown();
    }

    public void SetNvidiaReflex(int value)
    {
        // Implement Nvidia Reflex setting logic
        PlayerPrefs.SetInt("NvidiaReflex", value);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (postProcessMaterial != null)
        {
            Graphics.Blit(source, destination, postProcessMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void UpdateResolutionDropdown()
    {
#if UNITY_EDITOR
        // Simulate full-screen mode for testing in the editor
        FullScreenMode currentMode = (FullScreenMode)PlayerPrefs.GetInt("DisplayMode", (int)FullScreenMode.Windowed);
#else
        FullScreenMode currentMode = Screen.fullScreenMode;
#endif

        Debug.Log($"UpdateResolutionDropdown called. Current mode: {currentMode}");

        if (currentMode == FullScreenMode.ExclusiveFullScreen)
        {
            resolutionParent.SetActive(true);
            resolutionDropdown.ClearOptions();
            var options = new List<string>();
            foreach (var res in Screen.resolutions)
            {
                options.Add($"{res.width} x {res.height}");
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", 0);
            resolutionDropdown.RefreshShownValue();
        }
        else
        {
            resolutionParent.SetActive(false);
        }
    }
}
