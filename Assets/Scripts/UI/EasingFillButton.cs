using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnitySubCore.Easing;

public class EasingFillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private RectTransform _fillObject;
	[SerializeField]
	[Range(0f, 1f)]
	private float _targetScale = 0.2f;
	[SerializeField]
	private float _fillTime = 0.25f;
	private float nowTime = 0f;

	private bool isPointerOnButton = false;
	private RectTransform originTransform;

	public void Awake()
	{
		originTransform = (RectTransform)transform;

		return ;
	}

	public void Update()
	{
		float easingTime;
		float easingScale;

		if (!isPointerOnButton && nowTime > 0)
		{
			nowTime -= Time.deltaTime;
			if (nowTime <= 0)
				nowTime = 0;
		}
		else if (isPointerOnButton && nowTime < _fillTime)
		{
			nowTime += Time.deltaTime;
			if (nowTime >= _fillTime)
				nowTime = _fillTime;
		}
		easingTime = nowTime / _fillTime;
		easingScale = SCEasing.EasingByType(EEasingType.InOutCubic, easingTime) * _targetScale;
		_fillObject.sizeDelta = new Vector2(originTransform.sizeDelta.x * easingScale, originTransform.sizeDelta.y);
		return ;
	}

	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
		return ;
	}

	public void ChangeScene(int idx)
	{
		SceneManager.LoadScene(idx);
		return ;
	}

	public void ExitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
		return;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		isPointerOnButton = true;
		return ;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isPointerOnButton = false;
		return ;
	}
}
