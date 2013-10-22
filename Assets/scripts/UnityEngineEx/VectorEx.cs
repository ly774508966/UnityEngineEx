using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityEngineEx
{
	public static class VectorEx
	{
		public static IEnumerable<Vector3> Circle(this Vector3 v, float Radius, int Sectors, float dA0)
		{
			float dA = 2 * Mathf.PI / Sectors;
			for (int i = 0; i < Sectors; i++) {
				float a = dA0 + i * dA;
				yield return v + Radius * (new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)));
			}
			yield break;
		}
	}
}

