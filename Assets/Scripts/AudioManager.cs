using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//배경 음악 출력을 담당하는 클래스
public class AudioManager : MonoBehaviour
{
	public static float volume = 1.0f;

	//배경음악
	[SerializeField]
	private AudioClip[]	_audioClips;

	//재생될 컴포넌트
	private AudioSource	audioSource;

	//중복된 노래가 연속으로 재생되지 않도록 하기 위한 변수
	private List<int>	audioList = new List<int>();
	private int			now;
	
	public static void SetWorldVolume(float vol)
	{
		volume = vol;
		return ;
	}

	// Start is called before the first frame update
	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		for (int i = 0; i < _audioClips.Length; i++)
			audioList.Add(i);
		SetVolume(volume);
		now = Random.Range(0, audioList.Count);
		audioList.Remove(now);
		return ;
	}

	// Update is called once per frame
	void Update()
	{
		if (!audioSource.isPlaying)
			PlayNextClip(audioList[Random.Range(0, audioList.Count)]);
		return ;
	}

	//다음 곡을 재생함
	void PlayNextClip(int idx)
	{
		audioSource.clip = _audioClips[idx];
		audioList.Add(now);
		audioList.Remove(idx);
		audioSource.Play();
		return ;
	}

	public void SetVolume(float volume)
	{
		audioSource.volume = volume;
		return ;
	}
}
