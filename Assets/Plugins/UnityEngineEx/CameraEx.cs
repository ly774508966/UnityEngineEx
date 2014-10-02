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
	}
}
