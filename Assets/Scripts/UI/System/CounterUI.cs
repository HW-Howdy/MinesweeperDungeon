using System.Collections;
using TMPro;
using UnityEngine;

public class CounterUI : MonoBehaviour
{
	public GameObject UIParent;
	[SerializeField]
	private TMP_Text textScore;
	[SerializeField]
	private TMP_Text textGrade;

	private Transform[] UIs;
	private TMP_Text[] tmps;
	private GameResultCounter counter;
	private int score;
	private char grade;

	public void Awake()
	{
		counter = new GameResultCounter();
		counter.LoadCount();
		UIs = new Transform[UIParent.transform.childCount];
		tmps = new TMP_Text[UIParent.transform.childCount];
		for (int i = 0; i < tmps.Length; i++)
		{
			UIs[i] = UIParent.transform.GetChild(i);
			tmps[i] = UIParent.transform.GetChild(i).GetComponentInChildren<TMP_Text>();
		}
		return ;
	}

	public void Start()
	{
		CalculateScore();
		UpdateUI();
		StartCoroutine(PopupUI());
		return ;
	}

	private void CalculateScore()
	{
		score = 0;
		score += 3 * counter.countCellFound;
		score -= 2 * counter.countCellMine;
		score += 5 * counter.countCellItem;
		score += 40 * counter.countFloorClear;
		score += 10 * counter.countFloorDeep;
		SelectGrade();
		return ;
	}

	private void SelectGrade()
	{
		if		(score < 0100)
			grade = 'F';
		else if (score < 0500)
			grade = 'E';
		else if (score < 1000)
			grade = 'D';
		else if (score < 1500)
			grade = 'C';
		else if (score < 3000)
			grade = 'B';
		else if (score < 4500)
			grade = 'A';
		else // (score >= 4500)
			grade = 'S';
		return ;
	}

	public void UpdateUI()
	{
		tmps[0].text = $"Found Cell :\n{counter.countCellFound}";
		tmps[1].text = $"Found Mine :\n{counter.countCellMine}";
		tmps[2].text = $"Found Item :\n{counter.countCellItem}";
		tmps[3].text = $"Clear Floor :\n{counter.countFloorClear}";
		tmps[4].text = $"Reach Floor :\nB{counter.countFloorDeep}F";
		tmps[5].text = $"Use Item :\n{counter.countUseItem}";
		tmps[6].text = $"Use Magic :\n{counter.countUseSkill}";
		textScore.text = $"Score\n{score}";
		textGrade.text = $"Grade\n{grade}";
		return ;
	}

	public IEnumerator PopupUI()
	{
		WaitForSeconds wait = new WaitForSeconds(0.5f);

		for (int i = 0; i < UIs.Length; i++)
		{
			UIs[i].gameObject.SetActive(false);
			textScore.gameObject.SetActive(false);
			textGrade.gameObject.SetActive(false);
		}
		for (int i = 0; i < UIs.Length; i++)
		{
			yield return (wait);
			UIs[i].gameObject.SetActive(true);
		}
		yield return (new WaitForSeconds(1f));
		textScore.gameObject.SetActive(true);
		yield return (wait);
		textGrade.gameObject.SetActive(true);
		yield break ;
	}
}

