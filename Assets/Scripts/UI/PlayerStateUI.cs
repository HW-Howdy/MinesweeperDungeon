using TMPro;
using UnityEngine;

public class PlayerStateUI : MonoBehaviour
{
	public TMP_Text tmpHealth;
	public TMP_Text tmpMana;

	public void Awake()
	{
		PlayerState.Instance.ActionAfterEditHealth += UpdateHealthUI;
		PlayerState.Instance.ActionAfterEditMana += UpdateManaUI;
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