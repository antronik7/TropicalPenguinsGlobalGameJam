using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
	public GameObject BlockPrefab;
	public Transform PlayZone;
	public List<Transform> ForbidenZones;
	public float SpawnHeight;
	public float SpawningRateInSeconds;

	public IEnumerator<Vector3> NextBlockPositionEnumerator;


	private Rect PlayZoneRect;

	private void Start()
	{
		StartCoroutine("SpawnBlockCoroutine");

		Vector3 v = PlayZone.localScale;
		PlayZoneRect.size = new Vector2(v.x, v.z);

		v = PlayZone.position;
		PlayZoneRect.center = new Vector2(v.x, v.z);

		Debug.Log($"PlayZoneRect: {PlayZoneRect}");

		NextBlockPositionEnumerator = BuildNextBlockEnumerator();
	}

	public IEnumerator<Vector3> BuildNextBlockEnumerator()
	{
		while (true)
		{
			Vector3 newPosition = new Vector3(PlayZoneRect.xMin + Random.value * PlayZoneRect.width, SpawnHeight, PlayZoneRect.yMin + Random.value * PlayZoneRect.height);
			bool bInForbidenZone = false;

			Debug.Log($"newPosition: {newPosition}");

			foreach (Transform t in ForbidenZones)
			{
				if (IsVectorInZone(newPosition, t))
				{
					bInForbidenZone = true;
					break;
				};
			}

			if (!bInForbidenZone)
				yield return newPosition;
		}
	}

	public bool IsVectorInZone(Vector3 v, Transform zone)
	{
		Rect zoneRect = Rect.zero;
		Vector3 vZone = zone.localScale;
		vZone.y = vZone.z;
		zoneRect.size = vZone;

		vZone = zone.position;
		vZone.y = vZone.z;
		zoneRect.center = vZone;

		return v.x > zoneRect.xMin && v.x < zoneRect.xMax && v.z > zoneRect.yMin && v.z < zoneRect.yMax;
	}

	public IEnumerator SpawnBlockCoroutine()
	{
		while (true)
		{
			yield return new WaitForSecondsRealtime(SpawningRateInSeconds);
			SpawnBlock();
		}
	}

	public void SpawnBlock()
	{
		NextBlockPositionEnumerator.MoveNext();
		Vector3 newPosition = NextBlockPositionEnumerator.Current;

		Debug.Log(newPosition);

		GameObject newInstance = Instantiate(BlockPrefab, newPosition, Quaternion.identity);
	}

	private void OnDrawGizmos()
	{
		//Gizmos.DrawCube(Vector3.zero, Vector3.one);

		Color prevColor = Gizmos.color;
		Color drawingColor = Color.red;
		drawingColor.a = 0.5f;
		Gizmos.color = drawingColor;

		foreach (Transform t in ForbidenZones)
		{
			if (t != null)
				Gizmos.DrawCube(t.position, t.localScale);
		}

		drawingColor = Color.green;
		drawingColor.a = 0.1f;
		Gizmos.color = drawingColor;

		if (PlayZone != null)
			Gizmos.DrawCube(PlayZone.position, PlayZone.localScale);

		Gizmos.color = prevColor;
	}
}