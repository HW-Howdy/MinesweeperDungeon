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
		return;
	}
}
