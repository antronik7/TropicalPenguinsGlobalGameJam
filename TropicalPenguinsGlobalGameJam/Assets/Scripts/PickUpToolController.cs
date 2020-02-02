﻿using System;
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
	[SerializeField]
	AK.Wwise.Event blockDropEvent;
	[SerializeField]
	AK.Wwise.Event wallHit;

	public PlayerController playerController;
	public Animator animator;

	//Variables
	public bool isHoldingShape = false;

	private bool m_IsTryingToPickup = false;
	private bool isTryingToPickUp
	{
		get => m_IsTryingToPickup;
		set
		{
			m_IsTryingToPickup = value;
			OnTryingToPickupChanged(m_IsTryingToPickup);
		}
	}
	public event Action<bool> OnTryingToPickupChanged;


	private GameObject shapeToPickUp;
	private GameObject holdedShape;
	private int counterBtnPress = 0;


	private void Awake()
	{
		//animator = transform.parent.GetComponentInChildren<Animator>();
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
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Enter");
		if (isTryingToPickUp || isHoldingShape || other.transform.GetComponent<HouseManager>() != null)
			return;

		//pose pas trop de questions
		if (other.transform.parent != null)
		{
			if (other.transform.parent.parent != null)
			{
				//empecher le joueur de pick up des morceaux de maison
				if (other.transform.parent.parent.GetComponent<HouseManager>() != null)
					return;
			}
		}

		if (other.transform.parent != null && other.transform.parent.GetComponent<Shape>() != null)
		{
			isTryingToPickUp = true;
			shapeToPickUp = other.transform.parent.gameObject;
		}

		if (other.CompareTag("Wall")) {
			wallHit.Post(gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.parent != null && other.transform.parent.gameObject == shapeToPickUp)
			ResetPickUp();
	}

	public void RequestShapePlacement()
	{
		if (isHoldingShape)
		{
			PlaceShape();
		}
	}

	public void RequestShapePickUp()
	{
		if (isTryingToPickUp && shapeToPickUp != null)
		{
			++counterBtnPress;

			if (counterBtnPress >= nbrPressToPickUp)
				PickUpShape();
		}
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
		holdedShape = shapeToPickUp;
		shapeToPickUp = null;

		ResetPickUp();

		blockPickupEvent.Post(gameObject);
		animator.SetBool("HasBlock", true);
		Debug.Log("HasBlock=true");

	}

	private void PlaceShape()
	{
		holdedShape.transform.parent = null;
		holdedShape.transform.position = holdedShape.transform.position - (Vector3.up / 2f);
		Rigidbody myRigbody = holdedShape.AddComponent<Rigidbody>();
		blockDropEvent.Post(gameObject);
		animator.SetBool("HasBlock", false);
		myRigbody.isKinematic = true;
		
		holdedShape = null;
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
		if (holdedShape != null)
			return holdedShape.GetComponent<Shape>();
		else
			return null;
	}
}
