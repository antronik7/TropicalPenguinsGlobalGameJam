using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
	public GameObject BlockPrefab;
	public Transform PlayZone;
	public List<Transform> ForbidenZones;
	public float SpawnHeight;
	public float SpawningRateInSeconds;

	public IEnumerator<Vector3> NextBlockPositionEnumerator;
	public IEnumerable<Bounds> ForbidenBounds;

	private Bounds PlayZoneBounds;

	private void Start()
	{
		StartCoroutine("SpawnBlockCoroutine");

		Vector3 v = PlayZone.localScale;
		v.y = 3 * SpawnHeight;
		PlayZoneBounds.size = v;

		v = PlayZone.position;
		v.y = 0;
		PlayZoneBounds.center = v;

		NextBlockPositionEnumerator = BuildNextBlockEnumerator();

		ForbidenBounds = ForbidenZones.Select(t =>
		{
			Bounds b = new Bounds();

			Vector3 v2 = t.localScale;
			v2.y = 3 * SpawnHeight;
			b.size = v2;

			v2 = t.position;
			v2.y = 0;
			b.center = v2;

			return b;
		});
	}

	public IEnumerator<Vector3> BuildNextBlockEnumerator()
	{
		while (true)
		{
			Vector3 newPosition = new Vector3(
				PlayZoneBounds.min.x + Random.value * PlayZoneBounds.size.x,
				SpawnHeight,
				PlayZoneBounds.min.z + Random.value * PlayZoneBounds.size.z
			);
			bool bInForbidenZone = false;

			foreach (Bounds b in ForbidenBounds)
			{
				if (b.Contains(newPosition))
				{
					bInForbidenZone = true;
					break;
				}
			}

			if (!bInForbidenZone)
				yield return newPosition;
		}
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