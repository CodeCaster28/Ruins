using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods{

	public static int CalculateLimited(this int value, int parameter, int topLimit, int bottomLimit = 0) {
		value += parameter;
		return value > topLimit ? topLimit : (value < 0 ? 0 : value);
	}
}
