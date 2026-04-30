using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public SettingsMenu settingsMenu;

    public FloatArraySO VolumeSO;

    public BoolSO FullScreen;

    public FloatSO SensitivitySO;

    public float[] VolumeSettings;
    public bool fullscreen;
    public float sensitivity;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        SettingsSaveData settingsdata = LoadSystem.LoadSettingsData();
        if (settingsdata != null)
        {
            VolumeSO.Value = settingsdata.settingsdata.SoundSettings;
            FullScreen.Value = settingsdata.settingsdata.FullScreen;
            SensitivitySO.Value = settingsdata.settingsdata.Sensitivity;
        }

        UpdateVars();
        settingsMenu.SetValues();
    }

    public void UpdateVars()
    {
        VolumeSettings[0] = settingsMenu.VolumeValues[0];
        VolumeSettings[1] = settingsMenu.VolumeValues[1];
        VolumeSettings[2] = settingsMenu.VolumeValues[2];
        fullscreen = settingsMenu.fullscreen;

        sensitivity = SensitivitySO.Value;
    }

    public void SaveSettings()
    {
        UpdateVars();
        SaveSystem.SaveSettings();
    }
}
