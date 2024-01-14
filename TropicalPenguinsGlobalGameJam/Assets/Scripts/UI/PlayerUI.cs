using UnityEngine;

public class PlayerUI : MonoBehaviour
{
	public Vector2 uiDisplacement;

	public PlayerController playerController { get; set; }
	public Canvas canvas { get; set; }

	private void Start()
	{
		canvas = transform.GetComponentInParent<Canvas>();
		canvas.worldCamera = Camera.main;

		playerController.pickUpController.OnTryingToPickupChanged += OnTryingToPickupChanged;

		gameObject.SetActive(false);
	}

	public void OnTryingToPickupChanged(bool b)
	{
		gameObject.SetActive(b);
		UpdatePos();
	}

	private void UpdatePos()
	{
		Vector3 screenPos = canvas.worldCamera.WorldToScreenPoint(playerController.transform.position);

		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out Vector2 movePos);

		transform.position = canvas.transform.TransformPoint(movePos + uiDisplacement);
	}

	private void FixedUpdate() => UpdatePos();
}