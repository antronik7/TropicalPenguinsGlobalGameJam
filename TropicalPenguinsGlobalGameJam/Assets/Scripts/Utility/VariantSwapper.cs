using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariantSwapper : MonoBehaviour
{
	public GameObject original;
	public GameObject variant;

	private GameObject variantInstance;

	void Awake()
	{
		original = this.gameObject;

	}

    // Start is called before the first frame update
    void Start()
    {
		variantInstance = Instantiate(variant, transform.position, transform.rotation);
		variantInstance.SetActive(false);

		EventManager.GameEnd.AddListener(() => SwitchToVariant());
	}

	// This cancer is really hack-ish :) x    D
	private void SwitchToVariant()
	{
		variant.SetActive(true);
		original.SetActive(false);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SwitchToVariant();
		}
	}
}
