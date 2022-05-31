using Assets.Scripts.UIs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModesDropDown_Handler : MonoBehaviour
{
    public Dropdown dropdown_obj;
    public Text BestScoreText;

    // Start is called before the first frame update
    void Start()
    {
        createModesDropdown();

        int int_mode = PlayerPrefs.GetInt(MainController.Prefs_Modes_Key, MainController.Prefs_Modes_DefIndex);
        dropdown_obj.value = int_mode;
        BestScoreText.text = "HIGH SCORE\n" + (getHighScore(int_mode));

        //Dropdown dropdown_obj = transform.GetComponent<Dropdown>();
        dropdown_obj.onValueChanged.AddListener(delegate { dropdownItemSelected(dropdown_obj); });
    }

    void createModesDropdown()
    {
        List<string> items = new List<string>();
        items.Add(StaticVariables.mode1);
        items.Add(StaticVariables.mode2);
        items.Add(StaticVariables.mode3);

        dropdown_obj.options.Clear();
        foreach (var item in items)
        {
            dropdown_obj.options.Add(new Dropdown.OptionData() { text = item });
        }
    }

    private void dropdownItemSelected(Dropdown dropdown)
    {
        //string selected = dropdown.options[dropdown.value].text;
        BestScoreText.text = "BEST SCORE\n" + (getHighScore(dropdown.value));
        PlayerPrefs.SetInt(MainController.Prefs_Modes_Key, dropdown.value);
        PlayerPrefs.Save();
    }

    private int getHighScore(int mode)
    {
        if (mode == 0)
            return PlayerPrefs.GetInt(MainController.Prefs_CalmKey_HS, MainController.Prefs_HS_DefValue);
        else if (mode == 1)
            return PlayerPrefs.GetInt(MainController.Prefs_NormalKey_HS, MainController.Prefs_HS_DefValue);
        else if (mode == 2)
            return PlayerPrefs.GetInt(MainController.Prefs_CrazyKey_HS, MainController.Prefs_HS_DefValue);
        else
            return PlayerPrefs.GetInt(MainController.Prefs_NormalKey_HS, MainController.Prefs_HS_DefValue);
    }
}
