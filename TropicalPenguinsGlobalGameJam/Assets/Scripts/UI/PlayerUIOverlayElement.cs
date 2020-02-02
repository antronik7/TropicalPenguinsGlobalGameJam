using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIOverlayElement : MonoBehaviour
{
	public TextMeshProUGUI textField;
	public Image icon;

	public void UpdateScore(int score)
	{
		textField.text = score.ToString();
	}
}