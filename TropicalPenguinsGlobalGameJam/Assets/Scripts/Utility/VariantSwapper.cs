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
		if (variant == null)
			return;
		variantInstance = Instantiate(variant, transform.position, transform.rotation);
		variantInstance.SetActive(false);

		EventManager.GameplayEnd.AddListener(() => SwitchToVariant());
	}

	// This cancer is really hack-ish :) x    D
	private void SwitchToVariant()
	{
        return;

		if (variant == null)
			return;
		variantInstance.SetActive(true);
		original.SetActive(false);
	}
}
