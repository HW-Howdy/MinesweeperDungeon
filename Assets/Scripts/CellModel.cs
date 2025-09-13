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

	public void SetLocate(int y, int x)
	{
		this.x = x;
		this.y = y;
		state = MineMapModel.Instance.GetCellState(y, x);
		UpdateUI();
		return ;
	}

	public void UpdateUI()
	{
		if (state == ECellState.Open)
		{
			_background.color = Color.white;
			_text.text = MineMapModel.Instance.GetCountResult(y, x).ToString();
		}
		else if (state == ECellState.FlagRed)
		{
			_background.color = Color.gray;
			_text.text = "<color=red>Red</color>";
		}
		else if (state == ECellState.FlagBlue)
		{
			_background.color = Color.gray;
			_text.text = "<color=blue>Blue</color>";
		}
		else if (state == ECellState.Hidden)
		{
			_background.color = Color.gray;
			_text.text = "";
		}
		return ;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (state == ECellState.Open)
			return ;
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if ((state & ECellState.FlagRed) == 0 && (state & ECellState.FlagBlue) == 0)
				MineMapModel.Instance.SetCellState(y, x, ECellState.Open, out state);
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			if ((state & ECellState.FlagRed) == 0 && (state & ECellState.FlagBlue) == 0)
			{
				MineMapModel.Instance.FlagCellState(y, x, ECellState.FlagRed, out state);
			}
			else if ((state & ECellState.FlagRed) == ECellState.FlagRed)
			{
				MineMapModel.Instance.FlagCellState(y, x, ECellState.FlagRed, out state);
				MineMapModel.Instance.FlagCellState(y, x, ECellState.FlagBlue, out state);
			}
			else if ((state & ECellState.FlagBlue) == ECellState.FlagBlue)
			{
				MineMapModel.Instance.FlagCellState(y, x, ECellState.FlagBlue, out state);
			}
		}
		UpdateUI();
		return ;
	}
}
