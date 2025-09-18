using TMPro;
using UnityEngine;

public class SweeperUI : MonoBehaviour
{
	public TMP_Text tmp;

	public void Awake()
	{
		CellModel.ActionPointerEnter += UpdateUI;
		CellModel.ActionPointerExit += RemoveUI;
		return ;
	}

	public void UpdateUI(short value)
	{
		value %= 100;
		tmp.text = $"<color=red>Mine : {value % 10}</color> | <color=blue>Item : {value / 10}</color>";
		return ;
	}

	public void RemoveUI()
	{
		tmp.text = "Sweeper";
		return ;
	}
}
