using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionUI : MonoBehaviour
{
	public void DoFullScreen(bool isFull)
	{
		Screen.fullScreen = isFull;
		return ;
	}
}
