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

	private Coroutine fadeRoutine;
	private Queue<IEnumerator> fadeQueue = new Queue<IEnumerator>();
	private bool isFading = false;

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
		}
		return ;
	}

	private void EnqueueFade(IEnumerator routine)
	{
		fadeQueue.Enqueue(routine);
		if (!isFading)
		{
			ProcessNext();
		}
		return ;
	}

	private void ProcessNext()
	{
		if (fadeQueue.Count > 0)
		{
			isFading = true;
			fadeRoutine = StartCoroutine(RunFade(fadeQueue.Dequeue()));
		}
		else
		{
			isFading = false;
			fadeRoutine = null;
		}
		return ;
	}

	private IEnumerator RunFade(IEnumerator routine)
	{
		yield return StartCoroutine(routine);
		isFading = false;
		ProcessNext();
		yield break ;
	}

	public void StartFadeIn(float second = 1f, Action callback = null)
	{
		EnqueueFade(FadeIn(second, callback));
		return ;
	}

	public void StartFadeOut(float second = 1f, Action callback = null)
	{
		EnqueueFade(FadeOut(second, callback));
		return;
	}

	public void LoadSceneWithFade(string sceneName, float second = 1f, Action callback = null)
	{
		EnqueueFade(FadeAndLoad(sceneName, second, callback));
		return ;
	}

	public void LoadSceneWithFade(int idx, float second = 1f, Action callback = null)
	{
		EnqueueFade(FadeAndLoad(idx, second, callback));
		return ;
	}

	public void StartFade(float second = 1f, Action callFade = null, Action callback = null)
	{
		EnqueueFade(Fade(second, callFade, callback));
		return ;
	}

	public Color SetColor(Color color)
	{
		color.a = _fadeImage.color.a;
		_fadeImage.color = color;
		return (color);
	}

	private void SetAlpha(float alpha)
	{
		Color c = _fadeImage.color;

		c.a = alpha;
		_fadeImage.color = c;
		return;
	}

	private IEnumerator Fade(float second, Action callFade, Action callback)
	{
		yield return FadeOut(second, callFade);
		yield return FadeIn(second, callback);
		yield break;
	}
	private IEnumerator FadeAndLoad(string sceneName, float second, Action callback)
	{
		yield return FadeOut(second);
		yield return LoadScene(sceneName);
		yield return FadeIn(second, callback);
		yield break;
	}

	private IEnumerator FadeAndLoad(int idx, float second, Action callback)
	{
		yield return FadeOut(second);
		yield return LoadScene(idx);
		yield return FadeIn(second, callback);
		yield break;
	}

	private IEnumerator FadeOut(float second, Action callback = null)
	{
		float t = 0f;

		while (t < second)
		{
			t += Time.deltaTime;
			float alpha = Mathf.Clamp01(t / second);
			SetAlpha(alpha);
			yield return (null);
		}
		callback?.Invoke();
		yield break ;
	}

	private IEnumerator FadeIn(float second, Action callback = null)
	{
		float t = 0f;

		while (t < second)
		{
			t += Time.deltaTime;
			float alpha = 1f - Mathf.Clamp01(t / second);
			SetAlpha(alpha);
			yield return (null);
		}
		callback?.Invoke();
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
}