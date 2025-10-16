using System;
using UnityEngine;
using UnitySubCore.Singleton;

public class ItemManager : AMonoSingleton<ItemManager>
{
	private int[] itemList = new int[9];

	public int[] Items { get => itemList; }

	public event Action ActionItemValueChanged;

	public void GetItem(int index, int amount = 1)
	{
		if (index < 0 || index >= itemList.Length)
			throw (new Exception("index error"));
		itemList[index] += amount;
		ActionItemValueChanged?.Invoke();
		return ;
	}

	public bool UseItem(int index)
	{
		if (index < 0 || index >= itemList.Length)
			throw (new Exception("index error"));
		if (itemList[index] == 0)
			return (false);
		itemList[index] -= 1;
		ItemEffect(index);
		ActionItemValueChanged?.Invoke();
		return (true);
	}

	public void ItemEffect(int index)
	{
		switch (index)
		{
			case 0:
				GameManager.Instance.MineState[0].damage -= 1;
				break ;
			case 1:
				GameManager.Instance.MineState[1].damage -= 1;
				break ;
			case 2:
				GameManager.Instance.MineState[2].damage -= 1;
				break ;
			case 3:
				PlayerState.Instance.AddHealth(1);
				break ;
			case 4:
				PlayerState.Instance.AddHealth(2);
				break ;
			case 5:
				PlayerState.Instance.AddHealth(5);
				break ;
			case 6:
				PlayerState.Instance.AddMana(1);
				break ;
			case 7:
				PlayerState.Instance.AddMana(2);
				break ;
			case 8:
				PlayerState.Instance.AddMana(3);
				break ;
			default:
				throw (new Exception("unknown item index"));
		}
		return ;
	}
}
