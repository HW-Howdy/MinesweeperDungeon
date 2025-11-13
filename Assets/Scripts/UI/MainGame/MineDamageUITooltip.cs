using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MineDamageUITooltip : MonoBehaviour, ITooltipUI
{
	[SerializeField]
	private TMP_Text[] _texts;
	private Image image;
	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		image = GetComponent<Image>();
		rectTransform.localScale = Vector3.zero;
		return ;
	}

	private void SetUI(int idx)
	{
		SMineState state = GameManager.Instance.MineState[idx];

		image.color = state.color * 0.78f;
		_texts[0].text = $"이 유형의 지뢰칸을 밝히면 {GameManager.Instance.MineState[idx].damage}의 피해를 입습니다.";
		return ;
	}

	private void Move(PointerEventData eventData)
	{
		transform.position = eventData.position + new Vector2(rectTransform.sizeDelta.x / 2 + 10, 0) - new Vector2(0, rectTransform.sizeDelta.y / 2);
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
