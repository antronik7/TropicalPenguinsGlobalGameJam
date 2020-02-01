using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{

    public AK.Wwise.Event PlayBackgroundMusic;
    public AK.Wwise.RTPC PercentVolume;

    [Range(0, 100)]
    public float musicVolume = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        PlayBackgroundMusic.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        PercentVolume.SetValue(gameObject, musicVolume);
    }
}
