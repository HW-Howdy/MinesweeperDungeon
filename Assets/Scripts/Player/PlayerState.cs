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

	public int MaxHP { get => (healthMax); }
	public int MaxMP { get => (manaMax); }

	public void SetHealth(int health)
	{
		if (health <= 0)
			return ;
		healthCurrent = health;
		healthMax = health;
		ActionAfterEditHealth?.Invoke(healthCurrent, healthMax);
		return;
	}

	public void SetMana(int mana)
	{
		if (mana <= 0)
			return;
		manaCurrent = mana;
		manaMax = mana;
		ActionAfterEditMana?.Invoke(manaCurrent, manaMax);
		return;
	}

	public void SetMaxHealth(int health)
	{
		if (health <= 0)
			return;
		healthMax = health;
		healthCurrent = Math.Min(healthCurrent, healthMax);
		ActionAfterEditHealth?.Invoke(healthCurrent, healthMax);
		return;
	}

	public void SetMaxMana(int mana)
	{
		if (mana <= 0)
			return;
		manaMax = mana;
		manaCurrent = Math.Min(manaCurrent, manaMax);
		ActionAfterEditMana?.Invoke(manaCurrent, manaMax);
		return;
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
