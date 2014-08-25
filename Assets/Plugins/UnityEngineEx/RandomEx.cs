using UnityEngine;

namespace UnityEngineEx
{
	public static class RandomEx
	{
		public static float RandomDiff(this float v)
		{
			return v + Random.Range(-0.1f * v, 0.1f * v);
		}
	}
}
