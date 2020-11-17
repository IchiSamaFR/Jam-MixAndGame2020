using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public TextMeshProUGUI inputsText;
    KeyCollection kc;
    
    void Start()
    {
        kc = KeyCollection.instance;
        if (PlayerPrefs.GetInt("inputs") == 0)
        {
            inputsText.text = "qwerty";
        }
        else
        {
            inputsText.text = "azerty";
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("arena");
    }

    public void ChangeInputs()
    {
        if (PlayerPrefs.GetInt("inputs") == 0)
        {
            PlayerPrefs.SetInt("inputs", 1);
            inputsText.text = "azerty";
        }
        else
        {
            PlayerPrefs.SetInt("inputs", 0);
            inputsText.text = "qwerty";
        }
        kc.Refresh();
    }

    public void Quit()
    {
        SceneManager.LoadScene("main_menu");
    }
}
