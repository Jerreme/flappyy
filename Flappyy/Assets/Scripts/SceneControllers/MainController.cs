using UnityEngine;

public class MainController : MonoBehaviour
{
    // PlayerPrefs constants
    public const int Prefs_HS_DefValue = 0;
    public const string Prefs_CalmKey_HS = "calm_hscore";
    public const string Prefs_NormalKey_HS = "normal_hscore";
    public const string Prefs_CrazyKey_HS = "crazy_score";

    public const string Prefs_ColorIndex_Key = "color_index";
    public const int Prefs_ColorIndex_DefaultValue = 1;

    public const string Prefs_Sounds_Key = "sounds";
    public const int Prefs_Sounds_DefaultValue = 1;

    public const string Prefs_Modes_Key = "modeDropdown_index";
    public const int Prefs_Modes_DefIndex = 1;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting; //.NeverSleep
        Application.targetFrameRate = 60;
    }
}
