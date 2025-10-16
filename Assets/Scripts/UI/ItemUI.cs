using TMPro;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
	public TMP_Text[] _itemText;

	public void Start()
	{
		ItemManager.Instance.ActionItemValueChanged += UpdateUI;
		UpdateUI();
		return ;
	}

	public void UpdateUI()
	{
		for (int i = 0; i < _itemText.Length; i++)
		{
			_itemText[i].text = $"{ItemManager.Instance.Items[i]:D2}";
		}
		return ;
	}

	public void UseItem(int index)
	{
		if (ItemManager.Instance.UseItem(index))
		{
			Debug.Log($"Use Item {index}");
		}
		return ;
	}
}

