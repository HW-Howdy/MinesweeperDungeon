using System;
using System.Collections.Generic;
using UnityEngine;
using UnitySubCore.Singleton;

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

[Serializable]
public struct SMineState
{
	public int damage;
	public Color color;

	public void SetValue(int damage, Color color)
	{
		this.damage = damage;
		this.color = color;
		return ;
	}
}

public class GameManager : AMonoSingleton<GameManager>
{
	public GameResultCounter counter;

	[SerializeField]
	private GameStateSO[] _gameStateSOs;
	private SGameState gameState;

	private SMineState[]	mineState = new SMineState[3];
	private int				foundCellCommon;
	public SGameState GameState { get => gameState; }
	public SMineState[] MineState { get => mineState; }

	public Action ActionNextFloor;
	public Action<int, int> ActionAfterUpdateFloor;
	public Action ActionQuestNextFloor;
	public Action ActionOpenCellAfter;
	public Action ActionClearFloorAfter;

	protected override void Awake()
	{
		base.Awake();
		mineState[0].SetValue(1, Color.magenta);
		mineState[1].SetValue(1, Color.cyan);
		mineState[2].SetValue(1, Color.yellow);
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
		gameState = _gameStateSOs[gameMode].GetGameState();
		ActionNextFloor = null;
		PlayerState.Instance.SetHealth(6);
		PlayerState.Instance.SetMana(4);
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
			int mine = UnityEngine.Random.Range(0, mineState.Length);

			counter.countCellMine++;
			MineMapModel.Instance.CanOpen = false;
			SceneFader.Instance.SetColor(mineState[mine].color);
			SceneFader.Instance.StartFade(0.15f, null, () => Damage(mine));
		}
		else if (value / 100 == 2)
		{
			int item = UnityEngine.Random.Range(0, ItemManager.Instance.Items.Length);

			counter.countCellItem++;
			ItemManager.Instance.GetItem(item);
		}
		else if (value / 100 == 3)
		{
			ActionQuestNextFloor?.Invoke();
		}
		else
		{
			FoundCellCommon();
		}
		counter.countCellFound++;
		ActionOpenCellAfter?.Invoke();
		return;
	}

	public void FoundCellCommon()
	{
		foundCellCommon++;
		if (foundCellCommon == MineMapModel.Instance.CommonCellCount)
		{
			SceneFader.Instance.SetColor(Color.white);
			SceneFader.Instance.StartFade(0.2f);
			counter.countFloorClear++;
			ActionClearFloorAfter?.Invoke();
			Debug.Log("Clear Floor!");
		}
		return ;
	}

	public void OpenCellForce(int y, int x)
	{
		short value = MineMapModel.Instance.GetCellValue(y, x);

		MineMapModel.Instance.OpenCellForce(y, x);
		if (value / 100 == 0)
			OpenCellEvent(value);
		else if (value / 100 == 1)
			counter.countCellMine++;
		ActionOpenCellAfter?.Invoke();
		return ;
	}

	public void Damage(int mine)
	{
		if (PlayerState.Instance.RemoveHealth(mineState[mine].damage++) <= 0)
		{
			EndGame();
			return ;
		}
		MineMapModel.Instance.CanOpen = true;
		ActionOpenCellAfter?.Invoke();
		return ;
	}

	public void NextFloor()
	{
		if (!MineMapModel.Instance.CanOpen)
			return ;
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
		MineMapModel.Instance.CanOpen = false;
		counter.SaveCount();
		SceneFader.Instance.SetColor(Color.black);
		SceneFader.Instance.LoadSceneWithFade(2);
		return ;
	}
}
