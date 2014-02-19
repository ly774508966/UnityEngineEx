using UnityEngine;
using UnityEngineEx;

namespace UnityEngineEx
{
	[ExecuteInEditMode]
	public class PrefabContainer : MonoBehaviour
	{
		[SerializeField] GameObject prefab;

		void Awake()
		{
			Execute();
		}

		public void Execute()
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
				gameObject.transform.Clear();
				gameObject.Instantiate(prefab, null).CallRecursive((GameObject o) => o.tag = "EditorOnly");
				prevPrefab = prefab;
			}
		}

		public void DoReset()
		{
			prevPrefab = null;
			Update();
		}
#endif
	}
}
