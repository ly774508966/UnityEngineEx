#if UNITY_EDITOR
using UnityEngine;
using UnityEngineEx;
using UnityEditor;
using System.Collections;


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
}
}
#endif
