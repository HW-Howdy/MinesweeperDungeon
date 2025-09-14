using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellModel : MonoBehaviour, IPointerClickHandler
{
	private int x;
	private int y;
	private ECellState state;

	[SerializeField]
	private Image _background;
	[SerializeField]
	private TMP_Text _text;

	public Vector2Int Locate { get => new Vector2Int(x, y); }

	public static Action ActionAfterClick;

	public void SetLocate(int y, int x)
	{
		this.x = x;
		this.y = y;
		state = MineMapModel.Instance.GetCellState(y, x);
		UpdateUI();
		return ;
	}

	public void ReloadState(int y, int x)
	{
		state = MineMapModel.Instance.GetCellState(y, x);
		UpdateUI();
		return;
	}

	public void UpdateUI()
	{
		if (IsCellState(state, ECellState.Open))
		{
			_background.color = Color.white;
			_text.text = MineMapModel.Instance.GetCountResult(y, x).ToString();
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

	public void OnPointerClick(PointerEventData eventData)
	{
		if (IsCellState(state, ECellState.Open))
			return ;
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			MineMapModel.Instance.OpenCell(y, x);
			ActionAfterClick?.Invoke();
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
