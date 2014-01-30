using UnityEngine;
using System;

namespace UnityEngineEx
{
	public static class BoundsEx
	{
		public static Rect GetRect(this Bounds bounds)
		{
			if (bounds.extents.IsZero())
				return RectEx.CreateEmptyRect();
			return RectEx.CreateEmptyRect().Extend(bounds.min).Extend(bounds.max);
		}
	}
}

