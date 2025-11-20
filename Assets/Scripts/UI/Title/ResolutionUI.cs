using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionUI : MonoBehaviour
{
	[SerializeField]
	private TMP_Dropdown _resolutionDropdown;

	private Resolution[] resolutions;

	public void Start()
	{
		List<string> options = new List<string>();
		int currentIndex = 0;

		resolutions = Screen.resolutions;
		_resolutionDropdown.ClearOptions();
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = $"{resolutions[i].width} X {resolutions[i].height}";

			if (resolutions[i].width == 800 &&
				resolutions[i].height == 600)
			{
				currentIndex = i;
			}
			options.Add(option);
		}
		_resolutionDropdown.AddOptions(options);
		_resolutionDropdown.value = currentIndex;
		_resolutionDropdown.RefreshShownValue();
		return ;
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];

		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		return ;
	}

	public void DoFullScreen(bool isFull)
	{
		Screen.fullScreen = isFull;
		return ;
	}
}
