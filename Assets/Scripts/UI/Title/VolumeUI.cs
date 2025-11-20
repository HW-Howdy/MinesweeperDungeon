using System;
using UnityEngine;
using UnityEngine.UI;
using UnitySubCore.Easing;

public class VolumeUI : MonoBehaviour
{
	[SerializeField]
	private Slider _sliderVolume;

	private AudioSource[] audioSources;

	private void Start()
	{
		audioSources = GameObject.FindObjectsOfType<AudioSource>();
	}

	public void ChangeVolume(float volume)
	{
		foreach (AudioSource source in audioSources)
		{
			source.volume = volume;
		}
		AudioManager.SetWorldVolume(volume);
		return ;
	}
}
