#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace UnityEditorEx
{
	public class SpriteExt : MonoBehaviour
	{
		[MenuItem("Sprite/Auto Layer", true)]
		static bool AutoLayerCheck(MenuCommand command)
		{
			GameObject root = Selection.activeObject as GameObject;

			return root != null;
		}

		[MenuItem("Sprite/Auto Layer")]
		static void AutoLayer(MenuCommand command)
		{
			int depth = 0;
			IDictionary<string, int> layers = new Dictionary<string, int>();

			GameObject root = Selection.activeObject as GameObject;
			var sr = root.GetComponent<SpriteRenderer>();
			if (sr != null) {
				layers[sr.sortingLayerName] = sr.sortingOrder;
			}

			ReadLayers(root, layers);
			UpdateLayer(root, layers, depth);
		}

		static void ReadLayers(GameObject current, IDictionary<string, int> layers)
		{
			var sr = current.GetComponent<SpriteRenderer>();
			if (sr != null) {
				if (layers.ContainsKey(sr.sortingLayerName)) {
					layers[sr.sortingLayerName] = Mathf.Min(sr.sortingOrder, layers[sr.sortingLayerName]);
				}
				else {
					layers[sr.sortingLayerName] = sr.sortingOrder;
				}
			}

			foreach (Transform child in current.transform) {
				ReadLayers(child.gameObject, layers);
			}
		}

		static void UpdateLayer(GameObject current, IDictionary<string, int> layers, int depth)
		{
			var sr = current.GetComponent<SpriteRenderer>();
			if (sr != null) {
				if (layers.ContainsKey(sr.sortingLayerName)) {
					sr.sortingOrder = layers[sr.sortingLayerName] + depth;
				}
				else {
					layers[sr.sortingLayerName] = sr.sortingOrder;
				}
			}

			foreach (Transform child in current.transform) {
				UpdateLayer(child.gameObject, layers, depth + 1);
			}
		}
	}
}
#endif
