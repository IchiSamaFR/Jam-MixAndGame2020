﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("arena");
    }

    public void Menu()
    {
        SceneManager.LoadScene("main_menu");
    }
}
