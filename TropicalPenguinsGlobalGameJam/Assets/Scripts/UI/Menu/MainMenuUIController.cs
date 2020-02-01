using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuUIController : MonoBehaviour
{
    public void OnPressPlay()
    {
        Debug.Log("play");
        MenuSpawnerManager menuSpawnerManager = MenuSpawnerManager.Instance;
        menuSpawnerManager.SwitchUI(menuSpawnerManager.GameUI);
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }
}
