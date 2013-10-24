using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityEngineEx
{
	public static class VectorEx
	{
		public static Vector3 Sub(this Vector3 l, Vector2 r)
		{
			return new Vector3(l.x - r.x, l.y - r.y, l.z);
		}

		public static bool IsZero(this Vector2 v)
		{
			return v.x == 0 && v.y == 0;
		}

		public static bool IsZero(this Vector3 v)
		{
			return v.x == 0 && v.y == 0 && v.z == 0;
		}

		public static bool IsZero(this Vector4 v)
		{
			return v.x == 0 && v.y == 0 && v.z == 0 && v.w == 0;
		}

		public static Vector4 ToPoint(this Vector2 v)
		{
			return new Vector4(v.x, v.y, 0, 1.0f);
		}

		public static Vector4 ToPoint(this Vector3 v)
		{
			return new Vector4(v.x, v.y, v.z, 1.0f);
		}

		public static IEnumerable<Vector3> Circle(this Vector3 v, float Radius, int Sectors)
		{
			return v.Circle(Radius, Sectors, 0);
		}

		public static IEnumerable<Vector3> Circle(this Vector3 v, float Radius, int Sectors, float dA0)
		{
			float dA = 2 * Mathf.PI / Sectors;
			for (int i = 0; i < Sectors; i++) {
				float a = dA0 + i * dA;
				yield return v + Radius * (new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0));
			}
			yield break;
		}

		public static IEnumerable<Vector2> Lerp(this Vector2 a, Vector2 b, float dT)
		{
			float t = 0;
			while (t < 1) {
				yield return Vector2.Lerp(a, b, t); t += dT;
			}
			yield return Vector2.Lerp(a, b, 1);
			yield break;
		}

		public static IEnumerable<Vector3> Lerp(this Vector3 a, Vector3 b, float dT)
		{
			float t = 0;
			while (t < 1) {
				yield return Vector3.Lerp(a, b, t); t += dT;
			}
			yield return Vector3.Lerp(a, b, 1);
			yield break;
		}

		public static IEnumerable<Vector4> Lerp(this Vector4 a, Vector4 b, float dT)
		{
			float t = 0;
			while (t < 1) {
				yield return Vector4.Lerp(a, b, t); t += dT;
			}
			yield return Vector4.Lerp(a, b, 1);
			yield break;
		}

		public static IEnumerable<Vector3> Slerp(this Vector3 a, Vector3 b, float dT)
		{
			float t = 0;
			while (t < 1) {
				yield return Vector3.Slerp(a, b, t); t += dT;
			}
			yield return Vector3.Slerp(a, b, 1);
			yield break;
		}
	}
}

