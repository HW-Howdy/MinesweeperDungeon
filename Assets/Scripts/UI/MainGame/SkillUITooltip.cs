using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUITooltip : MonoBehaviour, ITooltipUI
{
	[SerializeField]
	private TMP_Text[] _texts;
	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = Vector3.zero;
		return ;
	}

	private void SetUI(int idx)
	{
		_texts[0].text = SkillManager.Instance.skillDesc[idx];
		return ;
	}

	private void Move(PointerEventData eventData)
	{
		transform.position = eventData.position + new Vector2(rectTransform.sizeDelta.x / 2 + 10, 0) + new Vector2(0, rectTransform.sizeDelta.y / 2);
		return ;
	}

	public void Enter(PointerEventData eventData, int idx)
	{
		rectTransform.localScale = Vector3.one;
		SetUI(idx);
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
