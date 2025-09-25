using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CounterUI : MonoBehaviour
{
	public GameObject UIParent;

	private Transform[] UIs;
	private TMP_Text[] tmps;
	private GameResultCounter counter;

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
		UpdateUI();
		StartCoroutine(PopupUI());
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
		tmps[6].text = $"Use Magic :\n{counter.countUseMagic}";
		return ;
	}

	public IEnumerator PopupUI()
	{
		WaitForSeconds wait = new WaitForSeconds(1f);

		for (int i = 0; i < UIs.Length; i++)
		{
			UIs[i].gameObject.SetActive(false);
		}
		for (int i = 0; i < UIs.Length; i++)
		{
			yield return (wait);
			UIs[i].gameObject.SetActive(true);
		}
		yield break ;
	}
}

