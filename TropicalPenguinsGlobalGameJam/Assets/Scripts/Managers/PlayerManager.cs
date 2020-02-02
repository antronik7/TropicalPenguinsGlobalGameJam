using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
	[SerializeField] private PlayerController playerPrefab;
	[SerializeField] private int playerCount;
	[SerializeField] private Transform[] spawnPoints;

	private List<PlayerController> players = new List<PlayerController>();
	private IdHelper idHelper;

	public int PlayerCount { get => playerCount; }

	// Start is called before the first frame update
	protected override void Awake()
	{
		idHelper = new IdHelper(playerCount);

		EventManager.GameplayReady.AddListener(OnGamePlayReady);
	}

	private void Initialize()
	{
		foreach (PlayerController player in players)
		{
			Destroy(player);
		}

		idHelper.Initialize(playerCount);
	}

	private void OnGamePlayStart(int playerCount)
	{
		if (playerCount > spawnPoints.Length)
		{
			throw new System.Exception("Not enough spawn points for the desired player count.");
		}

		for (int i = 0; i < playerCount; i++)
		{
			PlayerController player = SpawnPlayer(playerPrefab, spawnPoints[i]);
			players.Add(player);
			EventManager.PlayerSpawn.Invoke(player);
		}

		EventManager.GameplayStart.RemoveListener(() => OnGamePlayStart(playerCount));
		EventManager.GameplayEnd.AddListener(OnGamePlayEnd);
	}

	private void OnGamePlayEnd()
	{
		EventManager.GameplayEnd.RemoveListener(OnGamePlayEnd);
		EventManager.GameplayReady.AddListener(OnGamePlayReady);
	}

	private void OnGamePlayReady()
	{
		EventManager.GameplayReady.RemoveListener(OnGamePlayReady);
		EventManager.GameplayStart.AddListener(() => OnGamePlayStart(playerCount));
	}

	private PlayerController SpawnPlayer(PlayerController playerPrefab, Transform spawnPoint)
	{
		PlayerController player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
		player.SetCameraController(Camera.main.transform.parent.GetComponent<CameraController>());
		player.SetId(idHelper.GetFreeId());
		return player;
	}

	private void DespawnPlayer(PlayerController pc)
	{
		idHelper.ReleaseId(pc.playerId);
		players.Remove(pc);
		Destroy(pc.gameObject);
	}

	void Update()
	{
		if (ReInput.players.GetSystemPlayer().GetButtonDown(RewiredConsts.Action.GameplayStart))
		{
			EventManager.GameplayStart.Invoke();
		}
	}
}
