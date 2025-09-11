using Easing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PointEasingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Vector3 sizeOrigin = Vector3.one;
	[SerializeField]
	private Vector3 sizeTarget = Vector3.one * 1.2f;
	[SerializeField]
	private float scaleTime = 0.25f;
	private float nowTime = 0f;

	private bool isPointerOnButton = false;

	public void Update()
	{
		if (!isPointerOnButton)
		{
			nowTime = 0f;
			transform.localScale = sizeOrigin;
		}
		else
		{
			float easingTime;
			float easingScale;

			nowTime += Time.deltaTime;
			if (nowTime > 2 * scaleTime)
				nowTime -= 2 * scaleTime;
			if (nowTime >= scaleTime)
				easingTime = (2 * scaleTime - nowTime) / scaleTime;
			else
				easingTime = nowTime / scaleTime;
			easingScale = EasingFunctions.EasingByType(EasingFunctions.EFunctionTypes.InOutCubic, easingTime);
			transform.localScale = Vector3.Lerp(sizeOrigin, sizeTarget, easingScale);
		}
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
