using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
	protected void Awake()
	{
		EventManager.PlayerSpawn.AddListener((player) => OnPlayerSpawn(player));
		EventManager.GameplayEnd.AddListener(() => FuckAllControls());
	}

	private void OnPlayerSpawn(PlayerController pc)
	{
		PickUpToolController pickUpTool = pc.GetComponentInChildren<PickUpToolController>(pc);

		Player player = ReInput.players.GetPlayer(pc.playerId);
		if (player != null)
		{
			var gameplayRuleSet = ReInput.mapping.GetControllerMapEnablerRuleSetInstance(RewiredConsts.MapEnablerRuleSet.Gameplay);
			ChangeControlScheme(player, ControlScheme.MainGameplay);

			player.AddInputEventDelegate((ctx) => pc.Move(ctx.GetAxis()), UpdateLoopType.FixedUpdate, RewiredConsts.Action.Move);
			player.AddInputEventDelegate((ctx) => pc.Rotate(ctx.GetAxis()), UpdateLoopType.FixedUpdate, RewiredConsts.Action.Rotate);
			player.AddInputEventDelegate((ctx) => pc.Dash(ctx.GetButtonDown()), UpdateLoopType.FixedUpdate, RewiredConsts.Action.Dash);
			player.AddInputEventDelegate((ctx) => pickUpTool.RequestShapePickUp(), UpdateLoopType.FixedUpdate, InputActionEventType.ButtonJustPressed, RewiredConsts.Action.PickUpBlock);
			player.AddInputEventDelegate((ctx) => pickUpTool.RequestShapePlacement(), UpdateLoopType.FixedUpdate, InputActionEventType.ButtonJustPressed, RewiredConsts.Action.DumpBlock);
		}
	}

	// To find the usable maps, refer to the RewiredConsts.MapEnablerRuleSet syntax
	public static void ChangeControlScheme(PlayerController pc, ControlScheme scheme)
	{
		Player player = ReInput.players.GetPlayer(pc.playerId);
		if (player != null)
		{
			player.controllers.maps.mapEnabler.ruleSets.Clear();
			player.controllers.maps.mapEnabler.ruleSets.Add(GetRuleSetFromScheme(scheme));
			player.controllers.maps.mapEnabler.Apply();
		}
	}

	public static void ChangeControlScheme(int playerId, ControlScheme scheme)
	{
		Player player = ReInput.players.GetPlayer(playerId);
		if (player != null)
		{
			player.controllers.maps.mapEnabler.ruleSets.Clear();
			player.controllers.maps.mapEnabler.ruleSets.Add(GetRuleSetFromScheme(scheme));
			player.controllers.maps.mapEnabler.Apply();
		}
	}

	public static void ChangeControlScheme(Player player, ControlScheme scheme)
	{
		player.controllers.maps.mapEnabler.ruleSets.Clear();
		player.controllers.maps.mapEnabler.ruleSets.Add(GetRuleSetFromScheme(scheme));
		player.controllers.maps.mapEnabler.Apply();
	}

	private static ControllerMapEnabler.RuleSet GetRuleSetFromScheme(ControlScheme scheme)
	{
		ControllerMapEnabler.RuleSet ruleset;
		switch (scheme)
		{
			case ControlScheme.MainGameplay:
				ruleset = ReInput.mapping.GetControllerMapEnablerRuleSetInstance(RewiredConsts.MapEnablerRuleSet.Gameplay);
				break;
			case ControlScheme.TetrisGameplay:
				ruleset = ReInput.mapping.GetControllerMapEnablerRuleSetInstance(RewiredConsts.MapEnablerRuleSet.TetrisGameplay);
				break;
			default:
				ruleset = ReInput.mapping.GetControllerMapEnablerRuleSetInstance(RewiredConsts.MapEnablerRuleSet.Gameplay);
				break;
		}

		return ruleset;
	}

	public static void FuckAllControls()
	{
		for (int i = 0; i < 4; i++)
		{
			Player player = ReInput.players.GetPlayer(i);
			player.controllers.maps.mapEnabler.ruleSets.Clear();
			player.controllers.maps.mapEnabler.Apply();
		}
	}
}

public enum ControlScheme { MainGameplay, TetrisGameplay }