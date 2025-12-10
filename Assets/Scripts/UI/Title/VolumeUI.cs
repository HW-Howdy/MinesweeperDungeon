using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
	[SerializeField]
	private Slider _sliderVolume;
	[SerializeField]
	private TMP_Text _tmpVolume;

	private AudioSource[] audioSources;

	private void Start()
	{
		audioSources = FindObjectsOfType<AudioSource>();
		_sliderVolume.value = AudioManager.volume;
		return ;
	}

	public void ChangeVolume(float volume)
	{
		foreach (AudioSource source in audioSources)
		{
			source.volume = volume;
		}
		_tmpVolume.text = $"{volume:F2}";
		AudioManager.SetWorldVolume(volume);
		return ;
	}
}
