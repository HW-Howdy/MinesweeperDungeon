using System;
using UnityEngine;
using UnitySubCore.Singleton;

[Serializable]
public struct SGameState
{
	public string modeName;
	public int cols;
	public int rows;
	public float mineRatio;
	public float mineHigh;
	public float itemRatio;
	public float itemLow;
	public short floorNow;
	public short floorMax;
}

public class GameManager : AMonoSingleton<GameManager>
{
	public GameResultCounter counter;

	[SerializeField]
	private SGameState[] _gameStates;
	private SGameState gameState;
	public SGameState GameState { get => gameState; }

	private int foundCellCommon;

	public Action ActionNextFloor;
	public Action ActionOpenCellAfter;

	protected override void Awake()
	{
		base.Awake();
		counter = new GameResultCounter();
		return;
	}

	public void Start()
	{
		if (PlayerPrefs.HasKey("gameMode"))
			StartNewGame((short)PlayerPrefs.GetInt("gameMode"));
		else
			StartNewGame(1);
		return;
	}

	public void StartNewGame(short gameMode)
	{
		counter.Clear();
		gameState = _gameStates[gameMode];
		ActionNextFloor = null;
		NextFloor();
		SetFloorNext(gameMode);
		return;
	}

	private void SetFloorNext(short gameMode)
	{
		if (gameMode == 0)
			ActionNextFloor = () => NextFloorChallenge();
		else if (gameMode == 1)
			ActionNextFloor = () => NextFloorEasy();
		else if (gameMode == 2)
			ActionNextFloor = () => NextFloorNormal();
		else if (gameMode == 3)
			ActionNextFloor = () => NextFloorHard();
		return;
	}

	private void NextFloorEasy()
	{
		if (gameState.mineRatio < gameState.mineHigh)
			gameState.mineRatio += 0.01f;
		if (gameState.itemRatio > gameState.itemLow)
			gameState.itemRatio -= 0.01f;
		if (gameState.floorNow % 5 == 1)
		{
			gameState.cols += 1;
			gameState.rows += 1;
		}
		return;
	}

	private void NextFloorNormal()
	{
		if (gameState.mineRatio < gameState.mineHigh)
			gameState.mineRatio += 0.01f;
		if (gameState.itemRatio > gameState.itemLow)
			gameState.itemRatio -= 0.01f;
		if (gameState.floorNow % 5 == 1)
		{
			gameState.cols += 1;
			gameState.rows += 1;
		}
		return;
	}

	private void NextFloorHard()
	{
		if (gameState.mineRatio < gameState.mineHigh)
			gameState.mineRatio += 0.01f;
		if (gameState.itemRatio > gameState.itemLow)
			gameState.itemRatio -= 0.01f;
		if (gameState.floorNow % 5 == 1)
		{
			gameState.cols += 2;
			gameState.rows += 2;
		}
		return;
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
		return;
	}

	/*
	 * 0-8		=> 근처 함정의 수
	 * 0X-8X	=> 근처 아이템 또는 출구의 수
	 * 100		=> 함정 m
	 * 200		=> 아이템 i
	 * 300		=> 다음층 n
	 */
	public void OpenCellEvent(short value)
	{
		if (value / 100 == 1)
		{
			counter.countCellMine++;
		}
		else if (value / 100 == 2)
		{
			counter.countCellItem++;
		}
		else if (value / 100 == 3)
		{
			NextFloor();
		}
		else
		{
			foundCellCommon++;
			if (foundCellCommon == MineMapModel.Instance.CommonCellCount)
			{
				//Debug.Log("Clear Floor!");
			}
		}
		counter.countCellFound++;
		ActionOpenCellAfter?.Invoke();
		return;
	}

	public void NextFloor()
	{
		counter.countFloorDeep = ++gameState.floorNow;
		if (gameState.floorNow > gameState.floorMax)
		{
			//Debug.Log("Game Clear!");
		}
		else
		{
			ActionNextFloor?.Invoke();
			MineMapModel.Instance.SetupMap(gameState.rows, gameState.cols, gameState.mineRatio, gameState.itemRatio);
			//Debug.Log(gameState.floorNow);
		}
		foundCellCommon = 0;
		return;
	}
}
