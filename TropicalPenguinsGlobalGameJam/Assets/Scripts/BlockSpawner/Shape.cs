using System;
using UnityEngine;

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

	public void Release()
	{
		Owner = null;
	}

	public void Destroy()
	{
		Destroy(this);
	}
}