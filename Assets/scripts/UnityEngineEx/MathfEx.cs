using UnityEngine;
using System;

namespace UnityEngineEx
{
	public static class MathfEx
	{
		public static int Repeat(int t, int length)
		{
			while (t < 0)
				t += length;

			return t % length;
		}
	}
}
