using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	private int x;
	private int y;
	private ECellState state;
	private bool canActClickEvent;

	[SerializeField]
	private Image _background;
	[SerializeField]
	private TMP_Text _text;

	public Vector2Int Location { get => new Vector2Int(x, y); }

	public static Action ActionAfterClick;
	public static Action<short> ActionPointerEnter;
	public static Action ActionPointerExit;

	public void SetAct(bool act)
	{
		canActClickEvent = act;

		return ;
	}

	public void SetLocate(int y, int x)
	{
		this.x = x;
		this.y = y;
		state = MineMapModel.Instance.GetCellState(y, x);
		UpdateUI();
		return ;
	}

	public ECellState GetState()
	{
		return (state);
	}

	public void ReloadState(int y, int x)
	{
		state = MineMapModel.Instance.GetCellState(y, x);
		UpdateUI();
		return;
	}

	public int[] GetLocate()
	{
		return (new int[] { x, y });
	}

	public void UpdateUI()
	{
		if (IsCellState(state, ECellState.Open))
		{
			_text.text = MineMapModel.Instance.GetCountResultAsChar(y, x).ToString();
			if (_text.text.Equals("m"))
				_background.color = Color.magenta;
			else if (_text.text.Equals("i"))
				_background.color = Color.cyan;
			else if (_text.text.Equals("n"))
				_background.color = Color.green;
			else
				_background.color = Color.white;
		}
		else if (IsCellState(state, ECellState.FlagRed))
		{
			_background.color = Color.gray;
			_text.text = "<color=red>Red</color>";
		}
		else if (IsCellState(state, ECellState.FlagBlue))
		{
			_background.color = Color.gray;
			_text.text = "<color=blue>Blue</color>";
		}
		else if (IsCellState(state, ECellState.Hidden))
		{
			_background.color = Color.gray;
			_text.text = "";
		}
		return ;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (IsCellState(state, ECellState.Open))
			ActionPointerEnter?.Invoke(MineMapModel.Instance.GetCellValue(y, x));
		return ;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		ActionPointerExit?.Invoke();
		return;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!canActClickEvent)
			return ;
		if (IsCellState(state, ECellState.Open))
		{
			if (eventData.button == PointerEventData.InputButton.Left && MineMapModel.Instance.GetCountResultAsChar(y, x) == 'n')
			{
				GameManager.Instance.ActionQuestNextFloor?.Invoke();
			}
			if (eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 2)
			{
				if (MineMapModel.Instance.CheckAround(y, x))
					ActionAfterClick?.Invoke();
			}
		}
		else if (eventData.button == PointerEventData.InputButton.Left)
		{
			MineMapModel.Instance.OpenCell(y, x);
			ActionAfterClick?.Invoke();
			ActionPointerEnter?.Invoke(MineMapModel.Instance.GetCellValue(y, x));
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			MineMapModel.Instance.FlagCell(y, x, out state);
			UpdateUI();
		}
		return ;
	}

	public bool IsCellState(ECellState target, ECellState state)
	{
		if (target == ECellState.Hidden && state == ECellState.Hidden)
			return (true);
		else if ((target & state) == ECellState.Hidden)
			return (false);
		else
			return (true);
	}
}
