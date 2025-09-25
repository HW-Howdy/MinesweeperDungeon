using System;
using Unity.VisualScripting;
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
	public Action<int, int> ActionAfterUpdateFloor;
	public Action ActionQuestNextFloor;
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
		PlayerState.Instance.SetHealth(6);
		PlayerState.Instance.SetMana(4);
		PlayerState.Instance.SpendMana(4);
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
		if (gameState.floorNow % 2 == 1)
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
		if (gameState.cols <= 20)
		{
			gameState.cols += 1;
			gameState.rows += 1;
		}
		return;
	}

	private void NextFloorChallenge()
	{
		if (gameState.mineRatio < gameState.mineHigh)
			gameState.mineRatio += 0.01f;
		if (gameState.itemRatio > gameState.itemLow)
			gameState.itemRatio -= 0.01f;
		if (gameState.cols <= 100)
		{
			gameState.cols += 1;
			gameState.rows += 1;
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
			PlayerState.Instance.RemoveHealth(1);
			counter.countCellMine++;
		}
		else if (value / 100 == 2)
		{
			PlayerState.Instance.AddMana(1);
			counter.countCellItem++;
		}
		else if (value / 100 == 3)
		{
			ActionQuestNextFloor?.Invoke();
		}
		else
		{
			foundCellCommon++;
			if (foundCellCommon == MineMapModel.Instance.CommonCellCount)
			{
				SceneFader.Instance.SetColor(Color.white);
				SceneFader.Instance.StartFade(0.2f);
				Debug.Log("Clear Floor!");
			}
		}
		counter.countCellFound++;
		ActionOpenCellAfter?.Invoke();
		return;
	}

	public void NextFloor()
	{
		counter.countFloorDeep = ++gameState.floorNow;
		if (gameState.floorNow == 1)
		{
			MineMapModel.Instance.SetupMap(gameState.rows, gameState.cols, gameState.mineRatio, gameState.itemRatio);
		}
		else if (gameState.floorNow > gameState.floorMax)
		{
			EndGame();
		}
		else
		{
			ActionNextFloor?.Invoke();
			SceneFader.Instance.SetColor(Color.black);
			SceneFader.Instance.StartFade(0.3f, () => MineMapModel.Instance.SetupMap(gameState.rows, gameState.cols, gameState.mineRatio, gameState.itemRatio));
		}
		ActionAfterUpdateFloor?.Invoke(gameState.floorNow, gameState.floorMax);
		foundCellCommon = 0;
		return;
	}

	public void EndGame()
	{
		counter.SaveCount();
		SceneFader.Instance.SetColor(Color.black);
		SceneFader.Instance.LoadSceneWithFade(2);
		return ;
	}
}
