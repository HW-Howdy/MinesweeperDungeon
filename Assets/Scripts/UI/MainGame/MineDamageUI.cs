using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineDamageUI : MonoBehaviour
{
	[SerializeField]
	private MineDamageUICell[] _mineDamageUICells;

	public void Awake()
	{
		GameManager.Instance.ActionOpenCellAfter += UpdateUI;
		ItemManager.Instance.ActionItemValueChanged += UpdateUI;
		return ;
	}

	public void OnDestroy()
	{
		GameManager.Instance.ActionOpenCellAfter -= UpdateUI;
		ItemManager.Instance.ActionItemValueChanged -= UpdateUI;
		return ;
	}

	public void Start()
	{
		for (int i = 0; i < _mineDamageUICells.Length; i++)
		{
			_mineDamageUICells[i].index = i;
		}
		UpdateUI();
		return ;
	}

	public void UpdateUI()
	{
		for (int i = 0; i < _mineDamageUICells.Length; i++)
		{
			_mineDamageUICells[i].UpdateUI();
		}
		return ;
	}
}
