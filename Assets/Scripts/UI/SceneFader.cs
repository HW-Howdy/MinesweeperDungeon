using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnitySubCore.Singleton;

public class SceneFader : AMonoSingleton<SceneFader>
{
	[SerializeField]
	private Image _fadeImage;
	[SerializeField]
	private float _fadeDuration = 1f;

	private Coroutine fadeRoutine;

	protected override void Awake()
	{
		base.Awake();
		if (transform.parent == null)
		{
			GameObject parent = new GameObject();
			Canvas canvas = parent.AddComponent<Canvas>();
			CanvasScaler canvasScaler = parent.AddComponent<CanvasScaler>();

			DontDestroyOnLoad(parent);
			transform.SetParent(parent.transform);
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = 1;
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.referenceResolution = new Vector2(800, 600);
			canvasScaler.referencePixelsPerUnit = 100;
		}
		if (_fadeImage == null)
		{
			_fadeImage = gameObject.AddComponent<Image>();

			RectTransform rect = (RectTransform)transform;

			rect.anchoredPosition = Vector3.zero;
			rect.anchorMin = Vector3.zero;
			rect.anchorMax = Vector3.one;
			_fadeImage.color = Color.black;
			_fadeDuration = 1f;
		}
		return ;
	}

	public void StartFadeIn()
	{
		if (fadeRoutine != null)
			StopCoroutine(fadeRoutine);
		fadeRoutine = StartCoroutine(FadeIn());
		return ;
	}

	public void StartFadeOut()
	{
		if (fadeRoutine != null)
			StopCoroutine(fadeRoutine);
		fadeRoutine = StartCoroutine(FadeOut());
		return ;
	}

	private void SetAlpha(float alpha)
	{
		Color c = _fadeImage.color;

		c.a = alpha;
		_fadeImage.color = c;
		return ;
	}

	public void LoadSceneWithFade(string sceneName)
	{
		StartCoroutine(FadeAndLoad(sceneName));
		return ;
	}

	public void LoadSceneWithFade(int idx)
	{
		StartCoroutine(FadeAndLoad(idx));
		return ;
	}

	private IEnumerator FadeOut()
	{
		float t = 0f;

		while (t < _fadeDuration)
		{
			t += Time.deltaTime;
			float alpha = Mathf.Clamp01(t / _fadeDuration);
			SetAlpha(alpha);
			yield return (null);
		}
		yield break ;
	}

	private IEnumerator FadeIn()
	{
		float t = 0f;

		while (t < _fadeDuration)
		{
			t += Time.deltaTime;
			float alpha = 1f - Mathf.Clamp01(t / _fadeDuration);
			SetAlpha(alpha);
			yield return (null);
		}
		yield break ;
	}

	private IEnumerator LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
		yield break ;
	}

	private IEnumerator LoadScene(int idx)
	{
		SceneManager.LoadScene(idx);
		yield break ;
	}

	private IEnumerator FadeAndLoad(string sceneName)
	{
		yield return FadeOut();
		yield return LoadScene(sceneName);
		yield return FadeIn();
		yield break ;
	}
	
	private IEnumerator FadeAndLoad(int idx)
	{
		yield return FadeOut();
		yield return LoadScene(idx);
		yield return FadeIn();
		yield break ;
	}
}