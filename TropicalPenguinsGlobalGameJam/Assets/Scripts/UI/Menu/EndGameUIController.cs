using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class EndGameUIController : MonoBehaviour
{
	public TextMeshProUGUI BlocksP1;
	public TextMeshProUGUI BlocksP2;
	public TextMeshProUGUI BlocksP3;
	public TextMeshProUGUI BlocksP4;
	public TextMeshProUGUI HousesP1;
	public TextMeshProUGUI HousesP2;
	public TextMeshProUGUI HousesP3;
	public TextMeshProUGUI HousesP4;

	public List<TextMeshProUGUI> Scores;

	private void Awake()
	{
		EventManager.GameplayEnd.AddListener(() =>
		{
			gameObject.SetActive(true);
			UpdateScores(GameManager.Instance.playerScores);
		});

		EventManager.GameplayReady.AddListener(() =>
		{
			gameObject.SetActive(false);
		});

		gameObject.SetActive(false);
	}
	public void UpdateBlocks(int blocksP1, int blocksP2, int blocksP3, int blocksP4)
	{
		BlocksP1.SetText(blocksP1.ToString());
		BlocksP2.SetText(blocksP2.ToString());
		BlocksP3.SetText(blocksP3.ToString());
		BlocksP4.SetText(blocksP4.ToString());
	}

	public void UpdateHouses(int houseP1, int houseP2, int houseP3, int houseP4)
	{
		HousesP1.SetText(houseP1.ToString());
		HousesP2.SetText(houseP2.ToString());
		HousesP3.SetText(houseP3.ToString());
		HousesP4.SetText(houseP4.ToString());
	}
	public void UpdateScores(IEnumerable<int> scores)
	{
		foreach (var (score, text) in scores.Zip(Scores, (f, s) => (f, s)))
		{
			text.text = score.ToString();
		}
	}
	public void ToMainMenu()
	{
		EventManager.GameplayReady.Invoke();
	}
}
