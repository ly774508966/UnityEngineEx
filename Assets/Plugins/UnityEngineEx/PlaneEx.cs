using UnityEngine;

namespace UnityEngineEx
{
	public static class PlaneEx
	{
		public static bool Intersect(this Plane plane, Vector3 a, Vector3 b, out Vector3 r, out float d)
		{
			Vector3 dir = b - a;
			float dirm = dir.magnitude;

			if (plane.Raycast(new Ray(a, dir), out d)) {
				if (d <= dirm) {
					d /= dirm;
					r = Vector3.Lerp(a, b, d);
					return true;
				}
			}

			d = float.NaN;
			r = VectorEx.Empty3;
			return false;
		}
	}
}

