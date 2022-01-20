﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCredits : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            LoadMenu();
            Debug.Log("Skip Credits");
        }
    }
}