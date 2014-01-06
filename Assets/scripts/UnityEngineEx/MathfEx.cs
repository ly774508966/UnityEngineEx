using UnityEngine;
using System;

namespace UnityEngineEx
{
	public static class MathfEx
	{
		/// <summary>
		/// Repeat value in range [0..length] similar to mod operator. Works for negative values.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static int Repeat(int t, int length)
		{
			while (t < 0)
				t += length;

			return t % length;
		}

		public static float Cbrt(float v)
		{
			if (v >= 0) {
				return Mathf.Pow(v, 1/3.0f);
			}
			else {
				return -Mathf.Pow(-v, 1/3.0f);
			}
		}
	}
}
