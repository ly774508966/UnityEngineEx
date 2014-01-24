#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


namespace UnityEditorEx
{
public class ProjectExt : MonoBehaviour
{
	[MenuItem("Assets/Create/Material From Texture")]
	public static void MaterialFromTexture()
	{
		foreach (Object o in Selection.objects) {
			string assetPath = AssetDatabase.GetAssetPath(o);
			assetPath = assetPath.Substring(0, assetPath.LastIndexOf('/'));

			var materail = new Material(Shader.Find("Unlit/Transparent"));
			materail.mainTexture = o as Texture2D;
			AssetDatabase.CreateAsset(materail, assetPath + "/" + o.name + ".mat");
		}
	}

	[MenuItem("Assets/Copy path to clipboard")]
	public static void CopyAssetPathToClipboard()
	{
		foreach (Object o in Selection.objects) {
			EditorGUIUtility.systemCopyBuffer = AssetDatabase.GetAssetPath(o);
		}
	}
}
}
#endif
