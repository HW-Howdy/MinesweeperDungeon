using System;
using UnityEngine;
using UnitySubCore.Singleton;

public class PlayerState : ASingleton<PlayerState>
{
	private int healthCurrent;
	private int healthMax;

	private int manaCurrent;
	private int manaMax;

	public Action<int, int> ActionAfterEditHealth;
	public Action<int, int> ActionAfterEditMana;

	public void SetHealth(int health)
	{
		healthCurrent = health;
		healthMax = health;
		ActionAfterEditHealth?.Invoke(healthCurrent, healthMax);
		return ;
	}

	public void SetMana(int mana)
	{
		manaCurrent = mana;
		manaMax = mana;
		ActionAfterEditMana?.Invoke(manaCurrent, manaMax);
		return ;
	}

	public int AddHealth(int add)
	{
		healthCurrent = Math.Min(healthCurrent + add, healthMax);
		ActionAfterEditHealth?.Invoke(healthCurrent, healthMax);
		return (healthCurrent);
	}

	public int RemoveHealth(int remove)
	{
		healthCurrent = Math.Max(healthCurrent - remove, 0);
		ActionAfterEditHealth?.Invoke(healthCurrent, healthMax);
		return (healthCurrent);
	}

	public int AddMana(int add)
	{
		manaCurrent = Math.Min(manaCurrent + add, manaMax);
		ActionAfterEditMana?.Invoke(manaCurrent, manaMax);
		return (manaCurrent);
	}

	public bool SpendMana(int remove)
	{
		if (manaCurrent < remove)
			return (false);
		manaCurrent = Math.Min(manaCurrent - remove, manaMax);
		ActionAfterEditMana?.Invoke(manaCurrent, manaMax);
		return (true);
	}
}
