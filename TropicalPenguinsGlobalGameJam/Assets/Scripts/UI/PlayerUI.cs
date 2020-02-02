using UnityEngine;

public class PlayerUI : MonoBehaviour
{
	public RectTransform rectTransform => (RectTransform)transform;

	public PlayerController playerController;
	private Canvas canvas;


	private void OnEnable()
	{
		canvas = rectTransform.GetComponentInParent<Canvas>();
	}
	private void FixedUpdate()
	{
		RectTransform rt = rectTransform;
		Vector3 screenPos = canvas.worldCamera.WorldToScreenPoint(playerController.transform.position);

		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out Vector2 movePos);

		transform.position = canvas.transform.TransformPoint(movePos);

		Debug.Log(playerController.transform.position, playerController);
		Debug.Log("Camera", canvas.worldCamera);
		Debug.Log("Canvas", canvas);
	}
}