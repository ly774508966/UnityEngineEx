using UnityEngine;

namespace UnityEngineEx
{
	public static class ResolutionEx
	{
		/// <summary>
		/// Returns resolution in Vector2 struct.
		/// </summary>
		/// <param name="resolution"></param>
		/// <returns></returns>
		public static Vector2 GetSize(this Resolution resolution)
		{
			return new Vector2(resolution.width, resolution.height);
		}
	}
}
