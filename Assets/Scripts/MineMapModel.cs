using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
 * 0-8		=> 근처 함정의 수
 * 0X-8X	=> 근처 아이템 또는 출구의 수
 * 100		=> 함정 m
 * 101		=> 아이템 i
 * 102		=> 다음층 n
 */

public enum ECellState : byte
{
	Hidden =	0b0000_0000,
	Open =		0b0000_0001,
	FlagRed =	0b0000_0010,
	FlagBlue =	0b0000_0100,
}

public class MineMapModel : ASingleton<MineMapModel>
{
	private char[,] originMap;
	private ECellState[,] stateMap;

	public int Cols { get; private set; }
	public int Rows { get; private set; }
	public int RareCellCount { get; private set; }
	public int Length { get => Rows * Cols; }

	public Action ActionResizeMapAfter;

	protected override void Awake()
	{
		base.Awake();
		return ;
	}

	public void SetupMap(int rows, int cols, float mine = 0.1f, float item = 1)
	{
		ResizeMap(rows, cols);
		ClearMap();
		FillupMap(mine, item);
		ShowMapLog();
		return ;
	}

	private void ResizeMap(int rows, int cols)
	{
		Cols = cols;
		Rows = rows;
		originMap = new char[Cols, Rows];
		stateMap = new ECellState[Cols, Rows];
		ActionResizeMapAfter?.Invoke();
		return;
	}

	/*
	 * 지뢰찾기 프로그램 참고
	 * 
	 * 초급: 9x9 보드에 10개의 지뢰 (약 12.3%).
	 * 중급: 16x16 보드에 40개의 지뢰 (약 15.6%).
	 * 고급: 30x16 보드에 99개의 지뢰 (약 20.6%).
	 */
	private void FillupMap(float mine, float item)
	{
		int mineCount;
		int itemCount;

		if (mine < 0 || item < 0)
			throw (new Exception("함정과 아이템의 비율 또는 수는 0 이하일 수 없음"));
		if (mine < 1)
			mineCount = (int)MathF.Ceiling(mine * originMap.Length);
		else
			mineCount = (int)mine;
		if (item < 1)
			itemCount = (int)MathF.Floor(item * originMap.Length);
		else
			itemCount = (int)item;
		if (mineCount + itemCount + 1 >= originMap.Length)
			throw (new Exception("특수칸은 맵 전체 칸 수보다 클 수 없음"));
		FillupMap(mineCount, itemCount);
		return ;
	}

	private void FillupMap(int mine, int item)
	{
		int i = 0;
		int x;
		int y;

		while (++i <= mine)
		{
			x = UnityEngine.Random.Range(0, Cols);
			y = UnityEngine.Random.Range(0, Rows);
			if (originMap[x, y] >= 100)
				--i;
			else
			{
				originMap[x, y] = (char)(100);
				AddRoundCell(x, y);
			}
		}
		i = 0;
		while (++i <= item)
		{
			x = UnityEngine.Random.Range(0, Cols);
			y = UnityEngine.Random.Range(0, Rows);
			if (originMap[x, y] >= 100)
				--i;
			else
			{
				originMap[x, y] = (char)(101);
				AddRoundCell(x, y, 10);
			}
		}
		i = 0;
		while (i < 1)
		{
			x = UnityEngine.Random.Range(0, Cols);
			y = UnityEngine.Random.Range(0, Rows);
			if (originMap[x, y] >= 100)
				continue ;
			else
			{
				originMap[x, y] = (char)(102);
				AddRoundCell(x, y, 10);
				i++;
			}
		}
		RareCellCount = mine + item + 1;
		return ;
	}

	private void AddRoundCell(int col, int row, int count = 1)
	{
		int xt = col + 1;
		int yt = row + 1;

		for (int i = col - 1; i <= xt; i++)
		{
			for (int j = row - 1; j <= yt; j++)
			{
				if (i < 0 || j < 0 || i >= Cols || j >= Rows)
					continue ;
				if (originMap[i, j] < 100)
					originMap[i, j] += (char)(count);
			}
		}
		return ;
	}

	private void ClearMap()
	{
		Array.Clear(originMap, 0, originMap.Length);
		Array.Clear(stateMap, 0, stateMap.Length);
		return ;
	}

	private void ShowMapLog()
	{
		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < Cols; i++)
		{
			for (int j = 0; j < Rows; j++)
			{
				sb.Append(GetCountResultAsChar(j, i));
				sb.Append('.');
			}
			sb.Append('\n');
		}
		Debug.Log(sb.ToString());
		return ;
	}

	public char GetCountResultAsChar(int y, int x)
	{
		char result;

		if (IsOverMap(y, x))
			throw (new Exception("맵의 범위를 넘어서는 접근"));
		result = originMap[y, x];
		if (result < 100)
			result = (char)(result / 10 + result % 10 + '0');
		else if (result == 100)
			result = 'm';
		else if (result == 101)
			result = 'i';
		else if (result == 102)
			result = 'n';
		return (result);
	}

	public char GetCellValue(int y, int x)
	{
		if (IsOverMap(y, x))
			throw (new Exception("맵의 범위를 넘어서는 접근"));
		return (originMap[y, x]);
	}

	public ECellState GetCellState(int y, int x)
	{
		if (IsOverMap(y, x))
			throw (new Exception("맵의 범위를 넘어서는 접근"));
		return (stateMap[y, x]);
	}

	private void FlagCellState(int y, int x, ECellState state, out ECellState result)
	{
		if ((stateMap[y, x] & state) == 0)
			stateMap[y, x] |= state;
		else
			stateMap[y, x] &= ~state;
		result = stateMap[y, x];
		return ;
	}

	private void SetCellState(int y, int x, ECellState state, out ECellState result)
	{
		stateMap[y, x] = state;
		result = stateMap[y, x];
		return ;
	}

	public void OpenCell(int y, int x)
	{
		ECellState target = GetCellState(y, x);

		if (IsCellState(target, ECellState.Open) || IsCellState(target, ECellState.FlagRed) || IsCellState(target, ECellState.FlagBlue))
		{
			return ;
		}
		OpenCellEvent(y, x);
		return ;
	}

	public void FlagCell(int y, int x, out ECellState state)
	{
		ECellState target = GetCellState(y, x);

		if (IsCellState(target, ECellState.Open))
		{
			state = target;
			return ;
		}
		if (!(IsCellState(target, ECellState.FlagRed) || IsCellState(target, ECellState.FlagBlue)) )
		{
			FlagCellState(y, x, ECellState.FlagRed, out state);
		}
		else if (IsCellState(target, ECellState.FlagRed) && !IsCellState(target, ECellState.FlagBlue) )
		{
			FlagCellState(y, x, ECellState.FlagRed, out state);
			FlagCellState(y, x, ECellState.FlagBlue, out state);
		}
		else
		{
			FlagCellState(y, x, ECellState.FlagBlue, out state);
		}
		return ;
	}

	public void OpenCellEvent(int y, int x)
	{
		char value;

		if (IsOverMap(y, x) || !IsCellState(GetCellState(y, x), ECellState.Hidden))
			return ;
		value = GetCellValue(y, x);
		SetCellState(y, x, ECellState.Open, out ECellState temp);
		GameManager.Instance.OpenCellEvent(value);
		if (value == 0)
		{
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
					OpenCellEvent(y + j, x + i);
			}
		}

		return ;
	}

	public bool CheckAround(int y, int x)
	{
		bool result = (CountAroundFlag(y, x) == (byte)(GetCountResultAsChar(y, x) - '0'));

		if (result)
		{
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
					OpenCellEvent(y + j, x + i);
			}
		}

		return (result);
	}

	private byte CountAroundFlag(int y, int x)
	{
		byte result = 0;

		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (IsOverMap(y + j, x + i))
					continue ;
				if (IsCellState(GetCellState(y + j, x + i), ECellState.FlagRed) || IsCellState(GetCellState(y + j, x + i), ECellState.FlagBlue))
					result += 1;
				else if (IsCellState(GetCellState(y + j, x + i), ECellState.Open) && GetCellValue(y + j, x + i) >= 100)
					result += 1;
			}
		}
		return (result);
	}

	public bool IsCellState(ECellState target, ECellState state)
	{
		if (target == ECellState.Hidden && state == ECellState.Hidden)
			return (true);
		else if ((target & state) == ECellState.Hidden)
			return (false);
		else
			return (true);
	}

	public bool IsOverMap(int y, int x)
	{
		if (y < 0 || x < 0 || y >= Cols || x >= Rows)
			return (true);
		else
			return (false);
	}
}
