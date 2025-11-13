using UnityEngine;
using UnityEngine.EventSystems;

public class GameModeSelectTargetUI : MonoBehaviour, ITooltipTargetUI
{
	[SerializeField]
	private int _index;
	[SerializeField]
	private GameObject _tooltipUI;
	private ITooltipUI tooltip;

	private void Awake()
	{
		tooltip = _tooltipUI.GetComponent<ITooltipUI>();
		return ;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		tooltip?.Enter(eventData, _index);
		return ;
	}

	public void OnPointerMove(PointerEventData eventData)
	{
		tooltip?.UpdateUI(eventData, _index);
		return;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tooltip?.Exit(eventData, _index);
		return;
	}
}
