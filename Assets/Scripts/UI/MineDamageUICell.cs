using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineDamageUICell : MonoBehaviour
{
	[SerializeField]
	private Image _image;
	[SerializeField]
	private TMP_Text _text;
	public int index;

	public void Awake()
	{
		return ;
	}

	public void Start()
	{
		return ;
	}

	public void UpdateUI()
	{
		SMineState[] state = GameManager.Instance.MineState;
		
		_image.color = state[index].color;
		_text.text = $"{state[index].damage:D2}";
		return ;
	}

	public void ReduceDamage()
	{
		GameManager.Instance.MineState[index].damage -= 1;
		UpdateUI();
		return ;
	}
}
