using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private PlayerUI playerUIPrefab;
	private Canvas canvas;

	private readonly Dictionary<PlayerController, PlayerUI> playerUIs = new Dictionary<PlayerController, PlayerUI>();
	private void Awake()
	{
		canvas = GetComponent<Canvas>();
		EventManager.PlayerSpawn.AddListener((c) =>
		{
			PlayerUI newUI = Instantiate(playerUIPrefab, Vector3.zero, canvas.transform.rotation, canvas.transform);
			newUI.playerController = c;
			newUI.canvas = canvas;

			playerUIs.Add(c, newUI);
		});

		EventManager.PlayerDespawn.AddListener((c) =>
		{
			Destroy(playerUIs[c]);
			playerUIs.Remove(c);
		});
	}
}