using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
	public TMP_Text tmpFloor;
	public TMP_Text tmpHealth;
	public TMP_Text tmpMana;

	public void Awake()
	{
		GameManager.Instance.ActionAfterUpdateFloor += UpdateFloorUI;
		PlayerState.Instance.ActionAfterEditHealth += UpdateHealthUI;
		PlayerState.Instance.ActionAfterEditMana += UpdateManaUI;
		return ;
	}

	public void OnDestroy()
	{
		GameManager.Instance.ActionAfterUpdateFloor -= UpdateFloorUI;
		PlayerState.Instance.ActionAfterEditHealth -= UpdateHealthUI;
		PlayerState.Instance.ActionAfterEditMana -= UpdateManaUI;
		return ;
	}

	public void UpdateFloorUI(int current, int max)
	{
		tmpFloor.text = $"B{current:D2}F / {max:D2}";
		return ;
	}

	public void UpdateHealthUI(int current, int max)
	{
		tmpHealth.text = $"{current:D2} / {max:D2}";
		return ;
	}

	public void UpdateManaUI(int current, int max)
	{
		tmpMana.text = $"{current:D2} / {max:D2}";
		return ;
	}
}