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

	public PlayerController Owner { get; set; }
	public bool IsPicked => Owner != null;

	public bool IsFlipped { get; set; } // Tell / Set if shape is on a symmetry
	public bool IsRotated { get; set; } // Tell / Set if shape is rotated 90deg
	public bool IsInverted { get; set; } // Tell / Set if shape is rotated 90deg
}