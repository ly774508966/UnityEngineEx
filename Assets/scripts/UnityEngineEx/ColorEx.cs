using UnityEngine;

namespace UnityEngineEx
{
	public static class ColorEx
	{
		public static Color Alpha(this Color color, float a)
		{
			return new Color(color.r, color.g, color.b, a);
		}
	}
}

