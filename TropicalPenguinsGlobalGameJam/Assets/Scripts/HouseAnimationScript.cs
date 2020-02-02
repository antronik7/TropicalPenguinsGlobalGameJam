using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseAnimationScript : MonoBehaviour
{
    [SerializeField]
    private float downY;

    [SerializeField]
    private float RespawnAnimTime = 5;

    [SerializeField]
    private float TimeUntilRespawn = 2;

    [SerializeField]
    private MeshRenderer RepairedHouse;

    [SerializeField]
    private MeshRenderer DestroyedHouse;

    [SerializeField]
    private ParticleSystem Pouf;

    [SerializeField]
    AK.Wwise.Event houseComplete;

    private float deltaY { get { return standardY - downY; } }

    private float tickDeltaY{get{ return deltaY * Time.deltaTime / (RespawnAnimTime / 2); } }

    private float standardY;

    private float respawnDelayCounter = 0;

    private bool playAnim;

    private float tempCounter = 0;
    private bool played = false;

    HouseAnimState curState = HouseAnimState.None;

    enum HouseAnimState
    {
        None,
        Repair,
        GoDown,
        GoUp
    }

    // Start is called before the first frame update
    void Start()
    {
        standardY = transform.localPosition.y;
    }

    public void Play()
    {
        playAnim = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playAnim)
        {
            switch (curState)
            {
                case HouseAnimState.None:
                    RepairTransition();
                    break;
                case HouseAnimState.Repair:
                    respawnDelayCounter += Time.deltaTime;
                    if (respawnDelayCounter >= TimeUntilRespawn)
                    {
                        GoDownTransition();
                    }
                    break;
                case HouseAnimState.GoDown:
                    Vector3 curPos = transform.localPosition;
                    curPos.y -= tickDeltaY;
                    transform.localPosition = curPos;
                    if (curPos.y <= downY)
                    {
                        GoUpTransition();
                    }
                    break;
                case HouseAnimState.GoUp:
                    Vector3 curPos2 = transform.localPosition;
                    curPos2.y += tickDeltaY;
                    if (curPos2.y >= standardY)
                    {
                        EndTransition();
                        curPos2.y = standardY;
                    }
                    transform.localPosition = curPos2;
                    break;
            }
        }
    }

    void RepairTransition()
    {
        Pouf.gameObject.SetActive(true);
        Pouf.Play();
        houseComplete.Post(gameObject);
        RepairedHouse.gameObject.SetActive(true);
        DestroyedHouse.gameObject.SetActive(false);
        curState = HouseAnimState.Repair;
    }

    void GoDownTransition()
    {
        respawnDelayCounter = 0;
        curState = HouseAnimState.GoDown;
    }

    void GoUpTransition()
    {
        DestroyedHouse.gameObject.SetActive(true);
        RepairedHouse.gameObject.SetActive(false);
        curState = HouseAnimState.GoUp;
    }

    void EndTransition()
    {
        curState = HouseAnimState.None;
        playAnim = false;
    }
}
