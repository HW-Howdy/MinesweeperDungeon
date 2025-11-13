using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUICell : MonoBehaviour, ITooltipTargetUI
{
	public int _index;


	[SerializeField]
	private GameObject _tooltipUI;
	private ITooltipUI tooltip;

	public void Awake()
	{
		tooltip = _tooltipUI.GetComponent<ITooltipUI>();
		return;
	}


	public void OnPointerEnter(PointerEventData eventData)
	{
		tooltip?.Enter(eventData, _index);
		return;
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

