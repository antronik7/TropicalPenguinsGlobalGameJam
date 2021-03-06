using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BlockSpawner : Singleton<BlockSpawner>
{
	[SerializeField]
	[FormerlySerializedAs("m_ShapesPrefab")]
	private List<Shape> m_ShapePrefabs;

	public Transform PlayZone;
	public List<Transform> ForbidenZones;
	public float SpawnHeight;
	public float SpawningRateInSeconds;
	public int MaxNumberOfBlocks = 10;

	public IEnumerator<Vector3> NextBlockPositionEnumerator;
	public IEnumerable<Bounds> ForbidenBounds;

	public Dictionary<ShapeType, Shape> ShapePrefabs;
	public Shape[] BlockInstances { get; private set; }

	private Bounds PlayZoneBounds;
	private Bounds ObjectBounds;

	public GameObject BlocksContainer { get; private set; }

	private void Start()
	{
		BlockInstances = new Shape[MaxNumberOfBlocks];

		NextBlockPositionEnumerator = BuildNextBlockEnumerator();

		Vector3 v = PlayZone.localScale;
		v.y = 3 * SpawnHeight;
		PlayZoneBounds.size = v;

		v = PlayZone.position;
		v.y = 0;
		PlayZoneBounds.center = v;

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

		BlocksContainer = new GameObject("BlocksContainer");
		BlocksContainer.transform.parent = transform;

		// Have a Dictionnary instead for constant time get
		ShapePrefabs = m_ShapePrefabs.ToDictionary(s => s.Type);
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
			ObjectBounds.center = newPosition;

			foreach (Bounds b in ForbidenBounds)
			{
				if (b.Intersects(ObjectBounds))
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

			int nextPositionAvailable = Array.FindIndex(BlockInstances, b => b == null);
			if (nextPositionAvailable < 0)
				continue;

			ShapeType randomType = (ShapeType)(int)Random.Range(0, (int)ShapeType.Count - Mathf.Epsilon);
			Shape newShape = Instantiate(ShapePrefabs[randomType], Vector3.zero, Quaternion.identity, BlocksContainer.transform);

			List<MeshRenderer> renderers = new List<MeshRenderer>();
			newShape.GetComponentsInChildren(renderers);
			MeshRenderer rendererComponent = GetComponent<MeshRenderer>();
			if (rendererComponent != null)
				renderers.Add(rendererComponent);

			ObjectBounds = renderers.Aggregate(
				new Bounds(newShape.transform.position, Vector3.zero),
				(b, r) => { b.Encapsulate(r.bounds); return b; }
			);

			NextBlockPositionEnumerator.MoveNext();
			Vector3 newPosition = NextBlockPositionEnumerator.Current;

			newShape.transform.position = newPosition;

			newShape.Index = nextPositionAvailable;
			BlockInstances[nextPositionAvailable] = newShape;
		}
	}

	public Shape SpawnBlock(ShapeType type, Shape from)
	{
		Shape newShape = null;
		newShape = Instantiate(ShapePrefabs[type], from.transform.position, from.transform.rotation, BlocksContainer.transform);
		newShape.Index = from.Index;
		BlockInstances[newShape.Index] = newShape;

		var fromRigid = from.GetComponent<Rigidbody>();
		var shapeRigid = newShape.GetComponent<Rigidbody>();
		if (fromRigid == null)
		{
			Destroy(shapeRigid);
		}
		else
		{
			shapeRigid.isKinematic = fromRigid.isKinematic;
		}
		return newShape;
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

	public void GetPositions(Vector2Int pivot)
	{
	}

    public void StartSpawningBlocks()
    {
        // Start Spawn Coroutine
        StartCoroutine("SpawnBlockCoroutine");
    }
}