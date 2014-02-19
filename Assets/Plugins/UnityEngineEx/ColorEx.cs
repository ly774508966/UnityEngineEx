using UnityEngine;

namespace UnityEngineEx
{
	public static class ColorEx
	{
		public static Color empty = new Color(float.NaN, float.NaN, float.NaN, float.NaN);

		/// <summary>
		/// Returns new Color with the Alpha value set.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		public static Color Alpha(this Color color, float a)
		{
			return new Color(color.r, color.g, color.b, a);
		}

		public static bool IsEmpty(this Color color)
		{
			return float.IsNaN(color.a) || float.IsNaN(color.r) || float.IsNaN(color.g) || float.IsNaN(color.b);
		}

		public static Color FromHex(long hex)
		{
			return new Color(((hex >> 16) & 0xff)/255.0f, ((hex >> 8) & 0xff)/255.0f, (hex & 0xff)/255.0f, ((hex >> 24) & 0xff)/255.0f);
		}

		public static string ToHexString(this Color color)
		{
			return ((int)(255 * color.r)).ToString("x2") + ((int)(255 * color.g)).ToString("x2") + ((int)(255 * color.b)).ToString("x2") + ((int)(255 * color.a)).ToString("x2");
		}
	}
}

