using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineDamageUI : MonoBehaviour
{
	[SerializeField]
	private Image[] _image;
	[SerializeField]
	private TMP_Text[] _text;

	public void Awake()
	{
		GameManager.Instance.ActionOpenCellAfter += UpdateUI;
		ItemManager.Instance.ActionItemValueChanged += UpdateUI;
		return ;
	}

	public void Start()
	{
		UpdateUI();
		return ;
	}

	public void UpdateUI()
	{
		SMineState[] state = GameManager.Instance.MineState;

		for (int i = 0; i < state.Length; i++)
		{
			_image[i].color = state[i].color;
			_text[i].text = $"{state[i].damage:D2}";
		}
		return ;
	}
}
