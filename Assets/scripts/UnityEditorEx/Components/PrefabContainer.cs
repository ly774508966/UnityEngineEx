using UnityEngine;
using UnityEngineEx;

namespace UnityEditorEx.Components
{
	[ExecuteInEditMode]
	public class PrefabContainer : MonoBehaviour
	{
		[SerializeField] GameObject prefab;

		void Awake()
		{
#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlaying)
				gameObject.Reinstantiate(prefab);
#else
			gameObject.Reinstantiate(prefab);
#endif
		}

#if UNITY_EDITOR
		GameObject prevPrefab = null;
		void Update()
		{
			if (prevPrefab != prefab) {
				gameObject.transform.ClearImmidiate();
				gameObject.Instantiate(prefab, null);
				prevPrefab = prefab;
			}
		}

		public void Reset()
		{
			prevPrefab = null;
			Update();
		}
#endif
	}
}
