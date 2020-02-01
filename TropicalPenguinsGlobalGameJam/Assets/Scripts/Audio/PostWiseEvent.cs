using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWiseEvent : MonoBehaviour
{

    public AK.Wwise.Event TractorEngine;
    public AK.Wwise.Event BlockRotation;
    public AK.Wwise.Event BlockPlacement;
    public AK.Wwise.Event WrongPlacement;
    public AK.Wwise.Event Reward;

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
            BlockPlacement.Post(gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            WrongPlacement.Post(gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Reward.Post(gameObject);
        }
    }
}
