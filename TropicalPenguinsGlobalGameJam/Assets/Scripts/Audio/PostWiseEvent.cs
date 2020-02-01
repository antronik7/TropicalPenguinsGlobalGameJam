using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWiseEvent : MonoBehaviour
{

    public AK.Wwise.Event TractorEngine;
    public AK.Wwise.Event BlockRotation;
    public AK.Wwise.Event BlockPlacementGood;
    public AK.Wwise.Event BlockPlacementWrong;
    public AK.Wwise.Event HouseComplete;
    public AK.Wwise.RTPC TractorPercentSpeed;
    public AK.Wwise.RTPC Rotation_Volume;

    public float rotationVolume;
    public float speed = 0;
    // Start is called before the first frame update
    void Start()
    {
        TractorEngine.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            BlockRotation.Post(gameObject);
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            BlockPlacementGood.Post(gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            BlockPlacementWrong.Post(gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            HouseComplete.Post(gameObject);
        }

        TractorPercentSpeed.SetValue(gameObject, speed);

        Rotation_Volume.SetValue(gameObject, rotationVolume);
    }
}
