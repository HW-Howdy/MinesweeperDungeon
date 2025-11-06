using System;
using UnityEngine;
using UnityEngine.UI;
using UnitySubCore.Easing;

public class ConfigUI : MonoBehaviour
{
	[SerializeField]
	private GameObject UI;

	private RectTransform rectUI;
	private Vector2 originSize;

	[SerializeField]
	private float showTime = 0.5f;
	private float nowTime = 0f;
	private bool isActive = false;

	public void Awake()
	{
		rectUI = UI.GetComponent<RectTransform>();
		originSize = rectUI.sizeDelta;
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
}
