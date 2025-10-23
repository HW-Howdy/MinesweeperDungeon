using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnitySubCore.Singleton;

public class SkillManager : AMonoSingleton<SkillManager>
{
	[SerializeField]
	private GraphicRaycaster _raycaster;
	[SerializeField]
	private EventSystem _eventSystem;

	private int[] skillCost = { 1, 3, 4, 6 };

	public bool nowUseSkill = false;
	private int skillID = 0;

	public void Start()
	{
		if (_raycaster == null)
		{
			_raycaster = GameObject.FindAnyObjectByType<GraphicRaycaster>();
		}
		if (_eventSystem == null)
		{
			_eventSystem = GameObject.FindAnyObjectByType<EventSystem>();
		}
		return ;
	}

	public void Update()
	{
		if (nowUseSkill)
			GetSkillPosition(skillID);
		return ;
	}

	public void GetSkillPosition(int index)
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastResult[] result = Raycast(Input.mousePosition);

			for (int i = 0; i < result.Length; i++)
			{
				if (result[i].gameObject != null)
				{
					if (result[i].gameObject.TryGetComponent<CellView>(out CellView cell))
					{
						EffectSkillOnCell(cell, index);
						break ;
					}
					if (result[i].gameObject.TryGetComponent<MineDamageUICell>(out MineDamageUICell ui))
					{
						ReduceDamage(ui);
						break ;
					}
				}
			}
			StartCoroutine(FlagSkillEnd());
			Debug.Log("Click After Skill");
		}
		return ;
	}

	private RaycastResult[] Raycast(Vector3 position)
	{
		PointerEventData pointerEventData;
		List<RaycastResult> raycastResults = new List<RaycastResult>();
		
		pointerEventData = new PointerEventData(_eventSystem)
		{
			position = position
		};

		raycastResults.Clear();
		_raycaster.Raycast(pointerEventData, raycastResults);

		return (raycastResults.ToArray());
	}

	public void EffectSkillOnCell(CellView cell, int index)
	{
		int[] position = cell.GetLocate();
		int x = position[0];
		int y = position[1];

		if (cell.IsCellState(cell.GetState(), ECellState.Open))
			return ;
		if (index == 0)
		{
			GameManager.Instance.OpenCellForce(y, x);
			cell.UpdateUI();
		}
		else if (index == 1)
		{
			MineMapModel.Instance.CrossMineCheck(y, x);
		}
		else
			return ;
		PlayerState.Instance.SpendMana(skillCost[skillID]);
		GameManager.Instance.counter.countUseSkill++;
		Debug.Log("Skill Effect Cell");
		return ;
	}

	public void ReduceDamage(MineDamageUICell uiCell)
	{
		if (skillID != 2)
			return ;
		uiCell.ReduceDamage();
		PlayerState.Instance.SpendMana(skillCost[skillID]);
		GameManager.Instance.counter.countUseSkill++;
		return ;
	}

	public bool UseSkill(int index)
	{
		if (index < 0 || index >= skillCost.Length)
			throw (new Exception("index error"));
		if (PlayerState.Instance.AddMana(0) < skillCost[index])
			return (false);
		StartCoroutine(SkillEffect(index));
		return (true);
	}

	public IEnumerator FlagSkillEnd()
	{
		while (Input.GetMouseButton(0))
			yield return (null);
		FlagUseSkill(false);
		yield break ;
	}

	public void FlagUseSkill(bool flag, int index = 0)
	{
		nowUseSkill = flag;
		MineMapModel.Instance.CanOpen = !flag;
		skillID = index;
		return ;
	}

	public IEnumerator SkillEffect(int index)
	{
		yield return (null);
		switch (index)
		{
			case 0:
				FlagUseSkill(true, 0);
				break ;
			case 1:
				FlagUseSkill(true, 1);
				break ;
			case 2:
				FlagUseSkill(true, 2);
				break ;
			case 3:
				PlayerState.Instance.AddHealth(3);
				PlayerState.Instance.SpendMana(skillCost[index]);
				GameManager.Instance.counter.countUseSkill++;
				break ;
			default:
				throw (new Exception("unknown skill index"));
		}
		Debug.Log($"Use Skill {index}");
		yield break ;
	}
}