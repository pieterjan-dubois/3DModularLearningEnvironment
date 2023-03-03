using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var dropdown = transform.GetComponent<Dropdown>();
        dropdown.options.Clear();

        List<string> levels = new List<string>();
        levels.Add("Level 1");
        levels.Add("Level 2");
        levels.Add("Level 3");
        levels.Add("Level 4");
        levels.Add("Level 5");

/*
        List<string> test = GetComponent<EditorDatabase>().GetLevels();

        foreach (string level in test)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = level });
        }
*/

        foreach (string level in levels)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = level });
        }

        DropdownValueChanged(dropdown);

        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    public void DropdownValueChanged(Dropdown change)
    {
        Debug.Log("Selected: " + change.value);
    }
}
