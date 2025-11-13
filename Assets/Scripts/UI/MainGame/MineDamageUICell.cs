using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MineDamageUICell : MonoBehaviour, ITooltipTargetUI
{
	[SerializeField]
	private Image _image;
	[SerializeField]
	private TMP_Text _text;
	public int index;

	[SerializeField]
	private GameObject _tooltipUI;
	private ITooltipUI tooltip;

	public void Awake()
	{
		tooltip = _tooltipUI.GetComponent<ITooltipUI>();
		return ;
	}

	public void Start()
	{
		return ;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		tooltip?.Enter(eventData, index);
		return;
	}

	public void OnPointerMove(PointerEventData eventData)
	{
		tooltip?.UpdateUI(eventData, index);
		return;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tooltip?.Exit(eventData, index);
		return;
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
		if (GameManager.Instance.MineState[index].damage > 1)
			GameManager.Instance.MineState[index].damage -= 1;
		UpdateUI();
		return ;
	}
}
