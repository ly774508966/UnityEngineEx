using System.Collections.Generic;
using UnityEngine;

namespace UnityEditorEx
{
	public static class TransformEx
	{
		public static Transform ClearImmediate(this Transform transform)
		{
			List<GameObject> objects = new List<GameObject>();
			foreach (Transform child in transform) {
				objects.Add(child.gameObject);
			}
			foreach (GameObject o in objects) {
				GameObject.DestroyImmediate(o);
			}

			return transform;
		}
	}
}
