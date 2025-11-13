using System;
using UnityEngine;
using UnityEngine.UI;
using UnitySubCore.Easing;

public class ClearFloorUI : MonoBehaviour
{
	[SerializeField]
	private GameObject UI;

	[SerializeField]
	private Button UpgradeHP;
	[SerializeField]
	private Button UpgradeMP;
	[SerializeField]
	private Button HealHP;
	[SerializeField]
	private Button HealMP;


	private RectTransform rectUI;
	private Vector2 originSize;

	[SerializeField]
	private float showTime = 0.5f;
	private float nowTime = 0f;
	private bool isActive = false;

	public void Awake()
	{
		rectUI = UI.GetComponent<RectTransform>();
		originSize = rectUI.sizeDelta;
		GameManager.Instance.ActionClearFloorAfter += SetActiveTrue;
		return;
	}

	public void Update()
	{
		Resize();
		return;
	}

	public void Resize()
	{
		nowTime += (-1 + 2 * Convert.ToInt32(isActive)) * Time.deltaTime;
		nowTime = Mathf.Clamp(nowTime, 0f, showTime);

		float easing = SCEasing.EasingByType(EEasingType.InOutQuint, nowTime / showTime);
		rectUI.localScale = easing * Vector2.one;
		return;
	}

	public void CheckBonusAccess()
	{
		UpgradeHP.interactable = (PlayerState.Instance.MaxHP < 10);
		UpgradeMP.interactable = (PlayerState.Instance.MaxMP < 6);
		return ;
	}

	public void SetActiveTrue()
	{
		isActive = true;
		gameObject.SetActive(true);
		CheckBonusAccess();
		return ;
	}

	public void AcceptBonus(int index)
	{
		if (index == 0)
		{
			PlayerState.Instance.SetMaxHealth(PlayerState.Instance.MaxHP + 1);
		}
		else if (index == 1)
		{
			PlayerState.Instance.SetMaxMana(PlayerState.Instance.MaxMP + 1);
		}
		else if (index == 2)
		{
			PlayerState.Instance.AddHealth((PlayerState.Instance.MaxHP + 1) / 2);
		}
		else if (index == 3)
		{
			PlayerState.Instance.AddMana(PlayerState.Instance.MaxMP);
		}
		return ;
	}

	public void Active(bool isActive)
	{
		this.isActive = isActive;
		return ;
	}
}
