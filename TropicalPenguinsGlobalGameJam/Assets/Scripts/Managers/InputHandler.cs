using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : Singleton<InputHandler>
{		
	protected override void Awake()
	{
		EventManager.PlayerSpawn.AddListener((player) => OnPlayerSpawn(player));
	}

	private void OnPlayerSpawn(PlayerController pc)
	{
		PickUpToolController pickUpTool = pc.GetComponentInChildren<PickUpToolController>(pc);

		Player player = ReInput.players.GetPlayer(pc.playerId);
		if (player != null)
		{
			var gameplayRuleSet = ReInput.mapping.GetControllerMapEnablerRuleSetInstance(RewiredConsts.MapEnablerRuleSet.Gameplay);
			ChangeControlScheme(player, gameplayRuleSet);

			player.AddInputEventDelegate((ctx) => pc.Move(ctx.GetAxis()), UpdateLoopType.FixedUpdate, RewiredConsts.Action.Move);
			player.AddInputEventDelegate((ctx) => pc.Rotate(ctx.GetAxis()), UpdateLoopType.FixedUpdate, RewiredConsts.Action.Rotate);
			player.AddInputEventDelegate((ctx) => pc.Dash(ctx.GetButtonDown()), UpdateLoopType.FixedUpdate, RewiredConsts.Action.Dash);
			player.AddInputEventDelegate((ctx) => pickUpTool.RequestShapePickUp(), UpdateLoopType.FixedUpdate, InputActionEventType.ButtonJustPressed, RewiredConsts.Action.PickUpBlock);
			player.AddInputEventDelegate((ctx) => pickUpTool.RequestShapePlacement(), UpdateLoopType.FixedUpdate, InputActionEventType.ButtonJustPressed, RewiredConsts.Action.DumpBlock);
		}
	}

	// To find the usable maps, refer to the RewiredConsts.MapEnablerRuleSet syntax
	public void ChangeControlScheme(PlayerController pc, ControllerMapEnabler.RuleSet desiredMap)
	{
		Player player = ReInput.players.GetPlayer(pc.playerId);
		if (player != null)
		{
			player.controllers.maps.mapEnabler.ruleSets.Clear();
			player.controllers.maps.mapEnabler.ruleSets.Add(desiredMap);
			player.controllers.maps.mapEnabler.Apply();
		}
	}

	public void ChangeControlScheme(Player player, ControllerMapEnabler.RuleSet desiredMap)
	{
		player.controllers.maps.mapEnabler.ruleSets.Clear();
		player.controllers.maps.mapEnabler.ruleSets.Add(desiredMap);
		player.controllers.maps.mapEnabler.Apply();
	}
}
