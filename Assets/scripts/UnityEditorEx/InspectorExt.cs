#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngineEx;
using SystemEx;
using System.Collections.Generic;


namespace UnityEditorEx
{
	public class InspectorExt : MonoBehaviour
	{
		[MenuItem("CONTEXT/Transform/Normalize Parent")]
		public static void NormalizeParent(MenuCommand command)
		{
			GameObject o = Selection.activeGameObject;

			var objectPosition = o.transform.localPosition;
			o.transform.localPosition = Vector3.zero;

			o.transform.parent.position += objectPosition;
			foreach (Transform child in o.transform.parent) {
				if (child.gameObject == o)
					continue;

				child.transform.localPosition -= objectPosition;
			}
		}


		[MenuItem("CONTEXT/Transform/Distribute Objects", true)]
		public static bool DistributeObjectsCehck(MenuCommand command)
		{
			if (Selection.gameObjects.Length <= 1)
				return false;
			return true;
		}

		static int skipDistributeObjects = 0;
		[MenuItem("CONTEXT/Transform/Distribute Objects")]
		public static void DistributeObjects(MenuCommand command)
		{
			if (skipDistributeObjects == 0) {
				skipDistributeObjects = Selection.gameObjects.Length;
			}

			if (skipDistributeObjects == Selection.gameObjects.Length) {
				var objects = new SortedList<Vector3, GameObject>(LambdaComparer.Create((Vector3 a, Vector3 b) => {
					if (a.y < b.y) return -1;
					if (a.y == b.y && a.x < b.x) return -1;
					if (a.y == b.y && a.x == b.x) return 0;
					return 1;
				}));

				foreach (var o in Selection.gameObjects) {
					objects.Add(o.transform.localPosition, o);
				}

				var os = objects.Values;
				Vector3 first = os[0].transform.localPosition;
				Vector3 last = os[objects.Count - 1].transform.localPosition;

				float x = first.x;
				float dx = (last.x - first.x) / (objects.Count - 1);

				foreach (var o in os) {
					o.transform.localPosition = o.transform.localPosition.X(x);
					x += dx;
				}
			}

			skipDistributeObjects--;
		}
	}
}
#endif
