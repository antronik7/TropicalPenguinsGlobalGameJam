using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpawnerManager : MonoBehaviour
{
    private static MenuSpawnerManager _instance;

    public static MenuSpawnerManager Instance { get { return _instance; } }

    public Object MainMenuUI;

    public Object GameUI;

    public Object EndUI;

    private Object CurrentUI;

    private void Start()
    {
        if(_instance != null && _instance != this)
        {
            Debug.Log("Spawned extra MenuSpawnerManager");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            SwitchUI(MainMenuUI);
        }
    }

    public void SwitchUI(Object ui)
    {
        if(CurrentUI != null)
        {
            Destroy(CurrentUI);
        }

        CurrentUI = Instantiate(ui);
    }
}
