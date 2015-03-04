using UnityEditor;
using UnityEngine;
using UnityEngineEx;


namespace UnityEditorEx
{
	public class EditorExt : MonoBehaviour
	{
		[MenuItem("File/Save All %&s")]
		static void SaveAll()
		{
			EditorApplication.SaveAssets();
			EditorApplication.SaveScene();
		}

	
		[MenuItem("GameObject/Copy path to clipboard %&c", true)]
		static bool CopyPathToClipboardCheck()
		{
			return Selection.activeGameObject != null;
		}

		[MenuItem("GameObject/Copy path to clipboard %&c")]
		static void CopyPathToClipboard()
		{
			string path = "";
			var current = Selection.activeGameObject.transform;

			while (current != null) {
				path = current.name + (path.Length > 0 ? "/" + path : ""); 
				current = current.parent;
			}

			EditorGUIUtility.systemCopyBuffer = path;
		}
	}
}
