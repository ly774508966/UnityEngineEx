using UnityEditor;
using UnityEngine;

namespace UnityEditorEx.Components
{
	[CustomEditor(typeof(PrefabContainer))]
	class PrefabContainerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("Reset")) {
				((PrefabContainer)target).Reset();
			}

			base.OnInspectorGUI();
		}
	}
}
