using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

[Serializable]
public struct SGameState
{
	public string modeName;
	public string modeDecs;
	public int cols;
	public int rows;
	public float mineRatio;
	public float mineHigh;
	public float itemRatio;
	public float itemLow;
	public short floorNow;
	public short floorMax;
}

public class GameManager : ASingleton<GameManager>
{
	public GameResultCounter counter;

	[SerializeField]
	private SGameState[]	_gameStates;
	private SGameState		gameState;
	public  SGameState		GameState { get => gameState; }

	public Action ActionNextFloor;
	public Action ActionOpenCellAfter;

	protected override void Awake()
	{
		base.Awake();
		counter = new GameResultCounter();
		return ;
	}

	public void Start()
	{
		StartNewGame(1);
		return ;
	}

	public void StartNewGame(short gameMode)
	{
		counter.Clear();
		gameState = _gameStates[gameMode];
		ActionNextFloor = null;
		NextFloor();
		SetFloorNext(gameMode);
		return ;
	}

	private void SetFloorNext(short gameMode)
	{
		if (gameMode == 0)
			ActionNextFloor = () => NextFloorChallenge();
		else if (gameMode == 1)
			ActionNextFloor = () => NextFloorEasy();
		else if (gameMode == 2)
			ActionNextFloor = () => NextFloorMiddle();
		else if (gameMode == 3)
			ActionNextFloor = () => NextFloorHard();
		return ;
	}

	private void NextFloorEasy()
	{
		if (gameState.mineRatio < gameState.mineHigh)
			gameState.mineRatio += 0.01f;
		if (gameState.itemRatio > gameState.itemLow)
			gameState.itemRatio -= 0.01f;
		if (gameState.floorNow / 5 == 0)
		{
			gameState.cols += 1;
			gameState.rows += 1;
		}
		return ;
	}

	private void NextFloorMiddle()
	{
		if (gameState.mineRatio < gameState.mineHigh)
			gameState.mineRatio += 0.01f;
		if (gameState.itemRatio > gameState.itemLow)
			gameState.itemRatio -= 0.01f;
		if (gameState.floorNow / 5 == 1)
		{
			gameState.cols += 1;
			gameState.rows += 1;
		}
		return ;
	}

	private void NextFloorHard()
	{
		if (gameState.mineRatio < gameState.mineHigh)
			gameState.mineRatio += 0.01f;
		if (gameState.itemRatio > gameState.itemLow)
			gameState.itemRatio -= 0.01f;
		if (gameState.floorNow / 5 == 1)
		{
			gameState.cols += 2;
			gameState.rows += 2;
		}
		return ;
	}

	private void NextFloorChallenge()
	{
		if (gameState.mineRatio < gameState.mineHigh)
			gameState.mineRatio += 0.01f;
		if (gameState.itemRatio > gameState.itemLow)
			gameState.itemRatio -= 0.01f;
		if (gameState.floorNow / 5 == 1)
		{
			gameState.cols += (gameState.floorNow / 20) + 1;
			gameState.rows += (gameState.floorNow / 20) + 1;
		}
		return ;
	}

	/*
	 * 0-8		=> 근처 함정의 수
	 * 0X-8X	=> 근처 아이템 또는 출구의 수
	 * 100		=> 함정 m
	 * 101		=> 아이템 i
	 * 102		=> 다음층 n
	 */
	public void OpenCellEvent(char value)
	{
		if (value == 100)
		{
			counter.countCellMine++;
		}
		else if (value == 101)
		{
			counter.countCellItem++;
		}
		else if (value == 102)
		{
			NextFloor();
		}
		counter.countCellFound++;
		ActionOpenCellAfter?.Invoke();
		return ;
	}

	public void NextFloor()
	{
		if (++gameState.floorNow > gameState.floorMax)
		{

		}
		else
		{
			ActionNextFloor?.Invoke();
			MineMapModel.Instance.SetupMap(gameState.rows, gameState.cols, gameState.mineRatio, gameState.itemRatio);
		}
		counter.countFloorDeep = gameState.floorNow;
		return ;
	}
}
