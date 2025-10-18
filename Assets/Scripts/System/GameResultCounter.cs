using UnityEngine;

public class GameResultCounter
{
	public int countCellFound;
	public int countCellMine;
	public int countCellItem;

	public int countFloorClear;
	public int countFloorDeep;

	public int countUseSkill;
	public int countUseItem;

	public GameResultCounter()
	{
		Clear();
		return ;
	}

	public void Clear()
	{
		countCellFound = 0;
		countCellMine = 0;
		countCellItem = 0;
		countFloorClear = 0;
		countFloorDeep = 0;
		countUseSkill = 0;
		countUseItem = 0;
		return ;
	}

	public void SaveCount()
	{
		PlayerPrefs.SetInt("countCellFound", countCellFound);
		PlayerPrefs.SetInt("countCellMine", countCellMine);
		PlayerPrefs.SetInt("countCellItem", countCellItem);
		PlayerPrefs.SetInt("countFloorClear", countFloorClear);
		PlayerPrefs.SetInt("countFloorDeep", countFloorDeep);
		PlayerPrefs.SetInt("countUseMagic", countUseSkill);
		PlayerPrefs.SetInt("countUseItem", countUseItem);
		return ;
	}

	public void LoadCount()
	{
		if (!PlayerPrefs.HasKey("countCellFound"))
			return ;
		countCellFound = PlayerPrefs.GetInt("countCellFound", countCellFound);
		countCellMine = PlayerPrefs.GetInt("countCellMine", countCellMine);
		countCellItem = PlayerPrefs.GetInt("countCellItem", countCellItem);
		countFloorClear =PlayerPrefs.GetInt("countFloorClear", countFloorClear);
		countFloorDeep=PlayerPrefs.GetInt("countFloorDeep", countFloorDeep);
		countUseSkill = PlayerPrefs.GetInt("countUseMagic", countUseSkill);
		countUseItem = PlayerPrefs.GetInt("countUseItem", countUseItem);
		return ;
	}
}
