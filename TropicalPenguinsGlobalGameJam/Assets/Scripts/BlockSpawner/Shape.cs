using System;
using UnityEngine;

using Random = UnityEngine.Random;

public enum ShapeType
{
	Block,     // .
	ShortLine, // ..
	MedLine,   // ...
	ShortL,    // :.
	LongLine,  // ....
	LongL,     // :...
	Square,    // ::
	T,         // .:.
	Zigzag,    // ·:.
	Count
}

public class Shape : MonoBehaviour
{
	public ShapeType Type;
	public int Index;

	private PlayerController m_Owner;
	public PlayerController Owner
	{
		get => m_Owner;
		set => transform.parent = (m_Owner = value)?.transform ?? BlockSpawner.Instance.BlocksContainer.transform;
	}
	public bool IsPicked => m_Owner != null;

	public bool IsFlipped { get; set; } // Tell / Set if shape is on a symmetry
	public bool IsRotated { get; set; } // Tell / Set if shape is rotated 90deg
	public bool IsInverted { get; set; } // Tell / Set if shape is rotated 90deg

	private void OnEnable()
	{
		transform.hasChanged = false;
	}

	private void Update()
	{
		if (transform.hasChanged)
		{
			Vector3 eulerAngles = transform.rotation.eulerAngles;
			double yAngleRemainer = Math.IEEERemainder(eulerAngles.y, 360);
			IsInverted = yAngleRemainer < -135 || yAngleRemainer > 135;

			yAngleRemainer = Math.Abs(yAngleRemainer);
			IsRotated = yAngleRemainer > 45 && yAngleRemainer < 135;

			IsFlipped = transform.localScale.x < 0;
		}

		transform.hasChanged = false;
	}

	public void Crumble(PlayerController controller = null)
	{
		double rand = Random.value;
		ShapeType newType = ShapeType.Count;

		switch (Type)
		{
			case ShapeType.Block:
				break;
			case ShapeType.ShortLine:
				newType = ShapeType.Block;
				break;
			case ShapeType.MedLine:
			case ShapeType.ShortL:
				newType = ShapeType.ShortLine;
				break;
			case ShapeType.LongLine:
			case ShapeType.LongL when rand < 0.5:
				newType = ShapeType.MedLine;
				break;
			case ShapeType.LongL:
			case ShapeType.Square:
			case ShapeType.Zigzag:
			case ShapeType.T when rand < 0.66f:
				newType = ShapeType.ShortL;
				break;
			case ShapeType.T:
				newType = ShapeType.MedLine;
				break;
			case ShapeType.Count:
				throw new ArgumentOutOfRangeException("Type", "A shape has a Count Type, this should not happen");
		}

		if (newType != ShapeType.Count)
		{
			Shape newShape = BlockSpawner.Instance.SpawnBlock(newType, this);
			if (controller != null)
			{
				newShape.Owner = controller;
				controller.pickUpController.PickUpShape(newShape);
			}
		}

		Destroy(gameObject);
	}

	public void Release()
	{
		Owner = null;
	}
}