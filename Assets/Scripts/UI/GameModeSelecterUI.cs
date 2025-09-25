using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnitySubCore;
using UnitySubCore.Easing;

public class GameModeSelecterUI : MonoBehaviour
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
		rectUI = GetComponent<RectTransform>();
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

		float easing = SCEasing.EasingByType(EEasingType.InOutQuad, nowTime / showTime);
		rectUI.localScale = easing * Vector2.one;
		return ;
	}

	public void Active(bool isActive)
	{
		this.isActive = isActive;
		return ;
	}

	public void SelectGameMode(int gameMode)
	{
		PlayerPrefs.SetInt("gameMode", gameMode);
		SceneFader.Instance.LoadSceneWithFade(1);
		return ;
	}
}
