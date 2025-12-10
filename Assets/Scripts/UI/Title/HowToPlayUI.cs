using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnitySubCore.Easing;

public class HowToPlayUI : MonoBehaviour
{
	[SerializeField]
	private GameObject UI;

	private RectTransform rectUI;
	private Vector2 originSize;

	[SerializeField]
	private float showTime = 0.5f;
	private float nowTime = 0f;
	private bool isActive = false;

	[SerializeField]
	private GameObject _buttonNext;
	[SerializeField]
	private GameObject _buttonPrevious;
	[SerializeField]
	private TMP_Text _textPage;

	[SerializeField]
	private Transform _pageListTr;
	private GameObject[] pages;
	private int nowPage = 0;

	public void Awake()
	{
		rectUI = UI.GetComponent<RectTransform>();
		originSize = rectUI.sizeDelta;
		return ;
	}

	public void Start()
	{
		pages = new GameObject[_pageListTr.childCount];
		for (int i = 0; i < pages.Length; i++)
		{
			pages[i] = _pageListTr.GetChild(i).gameObject;
		}
		SetPage(nowPage);
		return ;
	}

	public void Update()
	{
		Resize();
		return ;
	}

	public void Resize()
	{
		nowTime += (- 1 + 2 * Convert.ToInt32(isActive)) * Time.deltaTime;
		nowTime = Mathf.Clamp(nowTime, 0f, showTime);

		float easing = SCEasing.EasingByType(EEasingType.InOutQuint, nowTime / showTime);
		rectUI.localScale = easing * Vector2.one;
		return ;
	}

	public void Active(bool isActive)
	{
		this.isActive = isActive;
		gameObject.SetActive(true);
		return ;
	}

	public void ChangePage(bool isNext)
	{
		if (isNext)
			nowPage++;
		else
			nowPage--;
		SetPage(nowPage);
		return ;
	}

	public void SetPage(int idx)
	{
		for (int i = 0; i < pages.Length; i++)
		{
			pages[i].SetActive(false);
		}
		pages[idx].SetActive(true);
		_buttonNext.SetActive(idx < pages.Length - 1);
		_buttonPrevious.SetActive(idx > 0);
		_textPage.text = $"( {idx + 1} / {pages.Length} )";
		return ;
	}
}
