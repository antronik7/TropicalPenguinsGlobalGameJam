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

    private float deltaY { get { return standardY - downY; } }

    private float tickDeltaY{get{ return deltaY / (RespawnAnimTime / 2); } }

    private float standardY;

    private float respawnDelayCounter = 0;

    private bool playAnim;

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
        RepairedHouse.gameObject.SetActive(false);
        standardY = transform.localPosition.y;

        //Test
        Play();
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
                    //Play FX
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
        DestroyedHouse.gameObject.SetActive(false);
        RepairedHouse.gameObject.SetActive(true);
        curState = HouseAnimState.Repair;
    }

    void GoDownTransition()
    {
        respawnDelayCounter = 0;
        curState = HouseAnimState.GoDown;
    }

    void GoUpTransition()
    {
        RepairedHouse.gameObject.SetActive(false);
        DestroyedHouse.gameObject.SetActive(true);
        curState = HouseAnimState.GoUp;
    }

    void EndTransition()
    {
        curState = HouseAnimState.None;
        playAnim = false;
    }
}
