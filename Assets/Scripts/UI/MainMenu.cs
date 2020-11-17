using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI inputsText;
    public TextMeshProUGUI bestScore;

    private void Start()
    {
        if (PlayerPrefs.GetInt("inputs") == 0)
        {
            inputsText.text = "qwerty";
        }
        else
        {
            inputsText.text = "azerty";
        }

        bestScore.text = "best wave : " + PlayerPrefs.GetInt("best_wave").ToString();
    }

    public void PlayArena()
    {
        SceneManager.LoadScene("arena");
    }

    public void ChangeInputs()
    {
        if(PlayerPrefs.GetInt("inputs") == 0)
        {
            PlayerPrefs.SetInt("inputs", 1);
            inputsText.text = "azerty";
        }
        else
        {
            PlayerPrefs.SetInt("inputs", 0);
            inputsText.text = "qwerty";
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
