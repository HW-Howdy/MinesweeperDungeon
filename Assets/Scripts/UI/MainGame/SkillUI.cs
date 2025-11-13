using TMPro;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
	public void UseSkill(int index)
	{
		if (SkillManager.Instance.nowUseSkill)
			return ;
		if (SkillManager.Instance.UseSkill(index))
		{
			Debug.Log($"UI Skill {index}");
		}
		return ;
	}
}

