using UnityEngine;
using System;

namespace UnityEngineEx
{
	public static class RectEx
	{
		public static Rect CreateEmptyRect()
		{
			return new Rect(float.NaN, float.NaN, float.NaN, float.NaN);
		}

		public static bool IsEmpty(this Rect rect)
		{
			return float.IsNaN(rect.xMin) || float.IsNaN(rect.yMin);
		}

		public static bool IsZero(this Rect rect)
		{
			return rect.width == 0 || rect.height == 0;
		}

		public static Rect Extend(this Rect rect, Vector2 p)
		{
			if (rect.IsEmpty())
				return new Rect(p.x, p.y, 0, 0);
			if (rect.Contains(p))
				return rect;

			float xMin = Mathf.Min(rect.xMin, p.x);
			float yMin = Mathf.Min(rect.yMin, p.y);
			float xMax = Mathf.Max(rect.xMax, p.x);
			float yMax = Mathf.Max(rect.yMax, p.y);

			return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
		}

		public static Vector2 Normalize(this Rect rect, Vector2 v)
		{
			return new Vector2((v.x - rect.xMin) / rect.width, (v.y - rect.yMin) / rect.height);
		}
	}
}

