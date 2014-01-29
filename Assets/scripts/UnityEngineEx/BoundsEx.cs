using UnityEngine;
using System;

namespace UnityEngineEx
{
	public static class BoundsEx
	{
		public static Rect GetRect(this Bounds bounds)
		{
			return RectEx.CreateEmptyRect().Extend(bounds.min).Extend(bounds.max);
		}
	}
}

