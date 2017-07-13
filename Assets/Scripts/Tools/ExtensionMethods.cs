using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods{

	public static int CalculateLimited(this int value, int parameter, int topLimit, int bottomLimit = 0) {
		value += parameter;
		return value > topLimit ? topLimit : (value < 0 ? 0 : value);
	}

	public static void SetTriggerRequired(this Animator anim, string triggerName, string animName, int layerIndex = 0){
		if (anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(animName))
			anim.SetTrigger(triggerName);
	}

	public static void SetTriggerExcluded(this Animator anim, string triggerName, string animName, int layerIndex = 0) {
		if (!anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(animName))
			anim.SetTrigger(triggerName);
	}
}
