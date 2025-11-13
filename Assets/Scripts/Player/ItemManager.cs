using System;
using UnityEngine;
using UnitySubCore.Singleton;

public class ItemManager : ASingleton<ItemManager>
{
	private int[] itemList = new int[9];

	public int[] Items { get => itemList; }
	
	public string[] itemDesc = {
		"첫번쩨 지뢰의 데미지가 2 이상이라면, 1 감소시킵니다.", //0 - 데미지 감소 유형1
		"두번째 지뢰의 데미지가 2 이상이라면, 1 감소시킵니다.", //1 - 데미지 감소 유형2
		"세번째 지뢰의 데미지가 2 이상이라면, 1 감소시킵니다.", //2 - 데미지 감소 유형3
		"체력을 1 회복합니다.", //3 - 체력 회복 1
		"체력을 2 회복합니다.", //4 - 체력 회복 2
		"체력을 5 회복합니다.", //5 - 체력 회복 5
		"마력을 1 회복합니다.", //6 - 마력 회복 1
		"마력을 2 회복합니다.", //7 - 마력 회복 2
		"마력을 3 회복합니다."  //8 - 마력 회복 3
	};

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
				if (GameManager.Instance.MineState[0].damage > 1)
					GameManager.Instance.MineState[0].damage -= 1;
				break ;
			case 1:
				if (GameManager.Instance.MineState[1].damage > 1)
					GameManager.Instance.MineState[1].damage -= 1;
				break ;
			case 2:
				if (GameManager.Instance.MineState[2].damage > 1)
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
