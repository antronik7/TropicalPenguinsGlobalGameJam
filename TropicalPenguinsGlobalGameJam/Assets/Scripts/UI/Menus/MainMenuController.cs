using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuController : MonoBehaviour
{
    public void OnPressPlay()
    {
        Debug.Log("play");
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }
}
