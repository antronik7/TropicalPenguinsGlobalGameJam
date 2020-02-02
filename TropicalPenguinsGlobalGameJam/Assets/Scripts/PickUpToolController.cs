using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpToolController : MonoBehaviour
{
	//Values
	[SerializeField]
	int nbrPressToPickUp = 5;

	[SerializeField]
	AK.Wwise.Event blockPickupEvent;

	//Variables
	public bool isHoldingShape = false;

	private bool isTryingToPickUp = false;
	private GameObject shapeToPickUp;
	private int counterBtnPress = 0;

	public PlayerController playerController;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (isTryingToPickUp)
		{
			if (shapeToPickUp == null)
			{
				ResetPickUp();
			}
			else
			{
				if (Input.GetButtonDown("ButtonA"))
				{
					++counterBtnPress;

					if (counterBtnPress >= nbrPressToPickUp)
						PickUpShape();
				}
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
		Debug.Log("Enter");
		if (isTryingToPickUp || isHoldingShape)
			return;

		if (other.transform.parent != null && other.transform.parent.GetComponent<Shape>() != null)
		{
			isTryingToPickUp = true;
			shapeToPickUp = other.transform.parent.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.parent != null && other.transform.parent.gameObject == shapeToPickUp)
			ResetPickUp();
	}

	public void PickUpShape(Shape shapeOverride = null)
	{
		if (shapeOverride != null)
			shapeToPickUp = shapeOverride.gameObject;

		isHoldingShape = true;
		shapeToPickUp.transform.position = shapeToPickUp.transform.position + (Vector3.up / 2f);
		shapeToPickUp.transform.parent = transform;
		Destroy(shapeToPickUp.GetComponent<Rigidbody>());
		shapeToPickUp.GetComponent<Shape>().Owner = playerController;

		ResetPickUp();

		blockPickupEvent.Post(gameObject);
		//EventManager.BlockPickupSound.Invoke();
	}

	private void PlaceShape()
	{
		shapeToPickUp.transform.parent = null;
		shapeToPickUp.transform.position = shapeToPickUp.transform.position - (Vector3.up / 2f);
		Rigidbody myRigbody = shapeToPickUp.AddComponent<Rigidbody>();
		myRigbody.isKinematic = true;

		shapeToPickUp = null;
		isHoldingShape = false;
	}

	private void ResetPickUp()
	{
		counterBtnPress = 0;
		isTryingToPickUp = false;
		counterBtnPress = 0;
	}

	public Shape GetHoldedShape()
	{
		if (shapeToPickUp != null)
			return shapeToPickUp.GetComponent<Shape>();
		else
			return null;
	}
}
