using TMPro;
using UnityEngine;

public class MineMapSizeUI : MonoBehaviour
{
	public TMP_Text tmp;

	public void Start()
	{
		MineMapModel.Instance.ActionResizeMapAfter += UpdateUI;
		return ;
	}

	public void UpdateUI()
	{
		tmp.text = $"Map Size\n{MineMapModel.Instance.Cols:D2} X {MineMapModel.Instance.Rows:D2}";
		return;
	}
}
