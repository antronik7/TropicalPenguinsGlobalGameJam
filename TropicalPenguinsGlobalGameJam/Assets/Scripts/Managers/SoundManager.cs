using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AK.Wwise.Event BlockPickup;

    private void Awake()
    {
        EventManager.BlockPickupSound.AddListener(() => PlayBlockPickupSound());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayBlockPickupSound()
    {
        BlockPickup.Post(gameObject);
        Debug.Log("Pickup sound");
    }
}
