using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
    public void OnEndGame()
    {
        MenuSpawnerManager menuSpawnerManager = MenuSpawnerManager.Instance;
        menuSpawnerManager.SwitchUI(menuSpawnerManager.EndUI);
    }
}
