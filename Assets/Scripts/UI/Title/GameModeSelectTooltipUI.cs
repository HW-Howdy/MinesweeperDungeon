using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameModeSelectTooltipUI : MonoBehaviour, ITooltipUI
{
	[SerializeField]
	private GameStateSO[] _gameStateSOs;
	[SerializeField]
	private TMP_Text[] _texts;
	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = Vector3.zero;
		return ;
	}

	private void SetUI(SGameState state)
	{
		_texts[0].text = state.modeName;
		_texts[1].text = $"Initial Map Size :\n{state.cols} X {state.rows}";
		_texts[2].text = $"Initial Mine :\n{state.mineRatio:P0} -> {state.mineHigh:P0}";
		_texts[3].text = $"Initial Item :\n{state.itemRatio:P0} -> {state.itemLow:P0}";
		_texts[4].text = $"Target Floor :\n{state.floorMax:D2}F";
		return ;
	}

	private void Move(PointerEventData eventData)
	{
		transform.position = eventData.position + new Vector2(0, rectTransform.sizeDelta.y / 2);
		return ;
	}

	public void Enter(PointerEventData eventData, int idx)
	{
		rectTransform.localScale = Vector3.one;
		SetUI(_gameStateSOs[idx].GetGameState());
		Move(eventData);
		return ;
	}

	public void UpdateUI(PointerEventData eventData, int idx)
	{
		Move(eventData);
		return ;
	}

	public void Exit(PointerEventData eventData, int idx)
	{
		rectTransform.localScale = Vector3.zero;
		return ;
	}
}
