#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


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
	}
}
#endif
