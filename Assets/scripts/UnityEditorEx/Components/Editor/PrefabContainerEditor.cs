using UnityEditor;
using UnityEngine;
using UnityEngineEx;

namespace UnityEditorEx.Components
{
	[CustomEditor(typeof(PrefabContainer))]
	class PrefabContainerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("Reset")) {
				((PrefabContainer)target).DoReset();
			}

			base.OnInspectorGUI();
		}
	}
}
