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
	//public bool[,] shapeArray = new bool[4, 4];
}