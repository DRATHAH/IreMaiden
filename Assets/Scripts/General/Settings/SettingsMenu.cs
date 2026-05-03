using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsMenu : MonoBehaviour
{
    [Header("Brightness")]
    public Slider BrightnessSlider;
    public FloatSO BrightnessSO;

    public Volume GlobalVolume;

    private ColorAdjustments colorAdjustments;

    [Header("Sensitivity")]
    public Slider SensitivitySlider;
    public FloatSO SensitivitySO;
    public float SensitivityValue;
    public PlayerLocomotionManager player;

    public AudioMixer AM;

    //Settings Menu Stuff
    public float[] VolumeValues = new float[3] { 0, 0, 0 };
    public bool fullscreen;

    //SOs to apply the settings menu stuff to
    public FloatArraySO VolumeSO;
    public BoolSO FullScreen;

    private Resolution[] resolutions;

    //UI Stuff
    public Slider[] VolumeSliders = new Slider[3];
    public Toggle FullScreenToggle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        //VolumeSliders[0] = GameObject.Find("MasterVolSlider").GetComponent<Slider>();
        //VolumeSliders[1] = GameObject.Find("MusicVolSlider").GetComponent<Slider>();
        //VolumeSliders[2] = GameObject.Find("SFXVolSlider").GetComponent<Slider>();
        //FullScreenToggle = GameObject.Find("FullScreenToggle").GetComponent<Toggle>();

        if (GlobalVolume != null && GlobalVolume.profile != null)
            GlobalVolume.profile.TryGet(out colorAdjustments);

        SetValues();
    }

    public void MasterVolume(float masterVolume)
    {
        VolumeSO.Value[0] = masterVolume;
        VolumeValues[0] = masterVolume;
        AM.SetFloat("MasterVolume", Mathf.Log10(VolumeSO.Value[0]) * 20);
    }

    public void MusicVolume(float sliderValue)
    {
        VolumeSO.Value[1] = sliderValue;
        VolumeValues[1] = sliderValue;
        AM.SetFloat("MusicVolume", Mathf.Log10(VolumeSO.Value[1]) * 20);
    }

    public void SFXVolume(float sliderValue)
    {
        VolumeSO.Value[2] = sliderValue;
        VolumeValues[2] = sliderValue;
        AM.SetFloat("SFXVolume", Mathf.Log10(VolumeSO.Value[2]) * 20);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        FullScreen.Value = isFullscreen;
        fullscreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
    }

    public void SetValues()
    {
        VolumeSliders[0].value = VolumeSO.Value[0];
        VolumeValues[0] = VolumeSO.Value[0];
        AM.SetFloat("MasterVolume", Mathf.Log10(VolumeSO.Value[0]) * 20);
        VolumeSliders[1].value = VolumeSO.Value[1];
        VolumeValues[1] = VolumeSO.Value[1];
        AM.SetFloat("MusicVolume", Mathf.Log10(VolumeSO.Value[1]) * 20);
        VolumeSliders[2].value = VolumeSO.Value[2];
        VolumeValues[2] = VolumeSO.Value[2];
        AM.SetFloat("SFXVolume", Mathf.Log10(VolumeSO.Value[2]) * 20);
        FullScreenToggle.isOn = FullScreen.Value;
        Screen.fullScreen = FullScreen.Value;
        fullscreen = FullScreen.Value;

        SensitivitySlider.value = SensitivitySO.Value;
        SensitivityValue = SensitivitySO.Value;

        BrightnessSlider.value = BrightnessSO.Value;
        SetBrightness(BrightnessSO.Value);

        if (player != null)
        {
            player.sensitivity = SensitivitySO.Value;
        }

        BrightnessSlider.value = BrightnessSO.Value;


        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = BrightnessSO.Value;
        }
    }

    public void SetSensitivity(float value)
    {
        SensitivitySO.Value = value;
        SensitivityValue = value;

        if (player != null)
            player.sensitivity = value;
    }

    public void SetBrightness(float value)
    {
        BrightnessSO.Value = value;

        if (colorAdjustments != null)
            colorAdjustments.postExposure.value = value;
    }
}
