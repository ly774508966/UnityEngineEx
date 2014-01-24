using UnityEditor;
using UnityEngine;

namespace UnityEditorEx.Components
{
	//[CustomEditor(typeof(Transform))]
	class TransformEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			/*
			if (GUILayout.Button("Reset")) {
				((PrefabContainer)target).DoReset();
			}*/
		}
	}
}
