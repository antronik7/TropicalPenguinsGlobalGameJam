using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : Singleton<InputHandler>
{
	protected override void Awake()
	{
		EventManager.PlayerSpawn.AddListener((player) => OnPlayerSpawn(player.playerId));
	}

	private void OnPlayerSpawn(int id)
	{
		Player player = ReInput.players.GetPlayer(id);
		if (player != null)
		{
			player.AddInputEventDelegate((ctx) => SampleInputs(ctx.playerId), UpdateLoopType.FixedUpdate);
		}
	}

	private void SampleInputs(int playerId)
	{
		Player player = ReInput.players.GetPlayer(playerId);
		throw new System.Exception("Continue once you have player scripts from Antoine :)");
	}
}
