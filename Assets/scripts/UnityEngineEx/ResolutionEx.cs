using UnityEngine;

namespace UnityEngineEx
{
	public static class ResolutionEx
	{
		public static Vector2 GetSize(this Resolution resolution)
		{
			return new Vector2(resolution.width, resolution.height);
		}
	}
}
