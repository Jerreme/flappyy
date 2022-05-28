using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModesDropDown_Handler : MonoBehaviour
{
    public Dropdown dropdown_obj;

    // Start is called before the first frame update
    void Start()
    {
        createModesDropdown();
        dropdown_obj.value = PlayerPrefs.GetInt(MainController.Prefs_Modes_Key, MainController.Prefs_Modes_DefIndex);
        //Dropdown dropdown_obj = transform.GetComponent<Dropdown>();
        dropdown_obj.onValueChanged.AddListener(delegate { dropdownItemSelected(dropdown_obj); });

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }


    private const string mode1 = "Calm";
    private const string mode2 = "Normal";
    private const string mode3 = "Crazy";

    void createModesDropdown()
    {
        List<string> items = new List<string>();
        items.Add(mode1);
        items.Add(mode2);
        items.Add(mode3);

        dropdown_obj.options.Clear();
        foreach (var item in items)
        {
            dropdown_obj.options.Add(new Dropdown.OptionData() { text = item });
        }
    }

    private void dropdownItemSelected(Dropdown dropdown)
    {
        string selected = dropdown.options[dropdown.value].text;
        PlayerPrefs.SetInt(MainController.Prefs_Modes_Key, dropdown.value);
        PlayerPrefs.Save();
    }
}
