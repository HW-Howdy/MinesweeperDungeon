using UnityEngine;

public class GameResultCounter
{
	public int countCellFound;
	public int countCellMine;
	public int countCellItem;

	public int countFloorClear;
	public int countFloorDeep;

	public int countUseMagic;

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
		countUseMagic = 0;
		return ;
	}

	public void SaveCount()
	{
		PlayerPrefs.SetInt("countCellFound", countCellFound);
		PlayerPrefs.SetInt("countCellMine", countCellMine);
		PlayerPrefs.SetInt("countCellItem", countCellItem);
		PlayerPrefs.SetInt("countFloorClear", countFloorClear);
		PlayerPrefs.SetInt("countFloorDeep", countFloorDeep);
		PlayerPrefs.SetInt("countUseMagic", countUseMagic);
		return ;
	}

	public void LoadCount()
	{
		if (!PlayerPrefs.HasKey("countCellFound"))
			return ;
		PlayerPrefs.GetInt("countCellFound", countCellFound);
		PlayerPrefs.GetInt("countCellMine", countCellMine);
		PlayerPrefs.GetInt("countCellItem", countCellItem);
		PlayerPrefs.GetInt("countFloorClear", countFloorClear);
		PlayerPrefs.GetInt("countFloorDeep", countFloorDeep);
		PlayerPrefs.GetInt("countUseMagic", countUseMagic);
		return ;
	}
}
