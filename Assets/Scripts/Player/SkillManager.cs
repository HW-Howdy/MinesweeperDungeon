using System;
using UnitySubCore.Singleton;

public class SkillManager : AMonoSingleton<SkillManager>
{
	private int[] skillCost = { 1, 3, 4, 6 };


	public bool UseSkill(int index)
	{
		if (index < 0 || index >= skillCost.Length)
			throw (new Exception("index error"));
		if (PlayerState.Instance.AddMana(0) < skillCost[index])
			return (false);
		return (true);
	}

	public void SkillEffect(int index)
	{
		switch (index)
		{
			case 0:
				break ;
			case 1:
				break ;
			case 2:
				break ;
			case 3:
				break ;
			default:
				throw (new Exception("unknown skill index"));
		}
		return ;
	}
}