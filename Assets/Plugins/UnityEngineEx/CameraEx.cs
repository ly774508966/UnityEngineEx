using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityEngineEx
{
	public static class CameraEx
	{
		public static Vector2 GetPixelSize(this Camera c)
		{
			return new Vector2(c.pixelWidth, c.pixelHeight);
		}

		public static IEnumerator<Ray> EnumCornerRays(this Camera c)
		{
			yield return c.ScreenPointToRay(new Vector2(0, 0));
			yield return c.ScreenPointToRay(new Vector2(0, c.pixelHeight));
			yield return c.ScreenPointToRay(new Vector2(c.pixelWidth, c.pixelHeight));
			yield return c.ScreenPointToRay(new Vector2(c.pixelWidth, 0));
			yield break;
		}

		public static Ray[] GetCornerRays(this Camera c)
		{
			Ray[] result = new Ray[4];
			result[0] = c.ScreenPointToRay(new Vector2(0, 0));
			result[1] = c.ScreenPointToRay(new Vector2(0, c.pixelHeight));
			result[2] = c.ScreenPointToRay(new Vector2(c.pixelWidth, c.pixelHeight));
			result[3] = c.ScreenPointToRay(new Vector2(c.pixelWidth, 0));
			
			return result;
		}
	}
}
