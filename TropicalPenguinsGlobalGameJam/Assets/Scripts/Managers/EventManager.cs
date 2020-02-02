using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
	// EXEMPLE EVENTS

	public static GameEvent _ExampleEvent = new GameEvent();
	public static GameEvent<int> _ExampleIntEvent = new GameEvent<int>();
	public static GameEvent<Vector3, Quaternion, float, bool, System.Action, Matrix4x4, ushort, double> _ExampleRidiculousParametersEvent = new GameEvent<Vector3, Quaternion, float, bool, System.Action, Matrix4x4, ushort, double>();

	// EXAMPLE EVENTS

	public static GameEvent GameplayStart = new GameEvent();
	public static GameEvent<PlayerController> PlayerSpawn = new GameEvent<PlayerController>();

	// SOUND

	public static GameEvent BlockPickupSound = new GameEvent();
}
