using System;
using UnityEngine.EventSystems;

public interface ITooltipUI
{
	public void Enter(PointerEventData eventData, int idx);
	public void UpdateUI(PointerEventData eventData, int idx);
	public void Exit(PointerEventData eventData, int idx);
}
