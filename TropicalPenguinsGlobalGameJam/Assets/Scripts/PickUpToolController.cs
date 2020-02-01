using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpToolController : MonoBehaviour
{
    //Values
    [SerializeField]
    int nbrPressToPickUp = 5;

    //Variables
    public bool isHoldingShape = false;

    private bool isTryingToPickUp = false;
    private GameObject shapeToPickUp;
    private int counterBtnPress = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTryingToPickUp)
        {
            if (Input.GetButtonDown("ButtonA"))
            {
                ++counterBtnPress;

                if (counterBtnPress >= nbrPressToPickUp)
                    PickUpShape();
            }
        }

        if (isHoldingShape)
        {
            if (Input.GetButtonDown("ButtonX"))
                PlaceShape();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTryingToPickUp || isHoldingShape)
            return;

        if (other.transform.parent.GetComponent<Shape>() != null)
        {
            isTryingToPickUp = true;
            shapeToPickUp = other.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.gameObject == shapeToPickUp)
            ResetPickUp();
    }

    private void PickUpShape()
    {
        isHoldingShape = true;
        shapeToPickUp.transform.position = shapeToPickUp.transform.position + (Vector3.up / 2f);
        shapeToPickUp.transform.parent = transform;
        Destroy(shapeToPickUp.GetComponent<Rigidbody>());

        ResetPickUp();
    }

    private void PlaceShape()
    {
        shapeToPickUp.transform.parent = null;
        shapeToPickUp.transform.position = shapeToPickUp.transform.position - (Vector3.up / 2f);
        shapeToPickUp = null;
        isHoldingShape = false;
    }

    private void ResetPickUp()
    {
        counterBtnPress = 0;
        isTryingToPickUp = false;
        counterBtnPress = 0;
    }
}
