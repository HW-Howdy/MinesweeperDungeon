using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
	public TMP_Text textGameMode;
	public TMP_Text textFloor;
	public TMP_Text textCellOpen;
	public TMP_Text textCellMine;

	public void Start()
	{
		GameManager.Instance.ActionOpenCellAfter += UpdateUI;
		UpdateUI();
		return ;
	}

	public void UpdateUI()
	{
		textGameMode.text = GameManager.Instance.GameState.modeName;
		textFloor.text = GameManager.Instance.counter.countFloorDeep.ToString();
		textCellOpen.text = GameManager.Instance.counter.countCellFound.ToString();
		textCellMine.text = GameManager.Instance.counter.countCellMine.ToString();
		return ;
	}
}

