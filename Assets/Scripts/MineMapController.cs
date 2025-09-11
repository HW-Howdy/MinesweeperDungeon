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
 * 101		=> 다음 층으로 i
 * 102		=> 아이템 획득 n
 */
public class MineMapController : MonoBehaviour
{
	private char[,] originMap;

	public int Raws { get; private set; }
	public int Cols { get; private set; }

	public void Start()
	{
		ResizeMap(5, 5);
		FillupMap(0.2f, 0.05f);
		ShowMapLog();
	}

	public void ResizeMap(int raws, int cols)
	{
		Raws = raws;
		Cols = cols;
		originMap = new char[Raws, Cols];
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
			x = UnityEngine.Random.Range(0, Raws);
			y = UnityEngine.Random.Range(0, Cols);
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
			x = UnityEngine.Random.Range(0, Raws);
			y = UnityEngine.Random.Range(0, Cols);
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
			x = UnityEngine.Random.Range(0, Raws);
			y = UnityEngine.Random.Range(0, Cols);
			if (originMap[x, y] >= 100)
				continue ;
			else
			{
				originMap[x, y] = (char)(102);
				AddRoundCell(x, y, 10);
				i++;
			}
		}
		return;
	}

	private void AddRoundCell(int raw, int col, int count = 1)
	{
		int xt = raw + 1;
		int yt = col + 1;

		for (int i = raw - 1; i <= xt; i++)
		{
			for (int j = col - 1; j <= yt; j++)
			{
				if (i < 0 || j < 0 || i >= Raws || j >= Cols)
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
		return ;
	}

	private void ShowMapLog()
	{
		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < Raws; i++)
		{
			for (int j = 0; j < Cols; j++)
			{
				sb.Append(GetCountResult(i, j));
				sb.Append('\t');
			}
			sb.Append('\n');
		}
		Debug.Log(sb.ToString());
		return ;
	}

	public char GetCountResult(int raw, int col)
	{
		char result;

		if (raw < 0 || col < 0 || raw >= Raws || col >= Cols)
			throw (new Exception("맵의 범위를 넘어서는 접근"));
		result = originMap[raw, col];
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

	public char GetCellValue(int raw, int col)
	{
		if (raw < 0 || col < 0 || raw >= Raws || col >= Cols)
			throw (new Exception("맵의 범위를 넘어서는 접근"));
		return (originMap[raw, col]);
	}
}
