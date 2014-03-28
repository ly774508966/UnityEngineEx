using UnityEngine;

namespace UnityEngineEx
{
	public static class TextAnchorEx
	{
		/// <summary>
		/// Converts TextAnchor to a Sprite pivot.
		/// </summary>
		/// <param name="anchor"></param>
		/// <returns></returns>
		public static Vector2 ToPivot(this TextAnchor anchor)
		{
			float pivotx, pivoty;
			if (anchor < TextAnchor.MiddleLeft)
				pivoty = 1.0f;
			else if (anchor < TextAnchor.LowerLeft)
				pivoty = 0.5f;
			else
				pivoty = 0.0f;

			int a = (int)(anchor) % 3;
			if (a == 0)
				pivotx = 0.0f;
			else if (a == 1)
				pivotx = 0.5f;
			else
				pivotx = 1.0f;

			return new Vector2(pivotx, pivoty);
		}
	}
}
