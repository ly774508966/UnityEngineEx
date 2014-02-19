using System;
using System.Collections;
using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	public static class CorotineEx
	{
		static IEnumerator CoroutineWrapper(IEnumerator coroutine, GameObject controller)
		{
			while (coroutine.MoveNext()) {
				yield return true;
			}

			GameObject.Destroy(controller);
			yield break;
		}

		public static GameObject StartCoroutine(IEnumerator coroutine)
		{
			GameObject go = new GameObject();
			go.AddComponent<MonoBehaviour>().StartCoroutine(CoroutineWrapper(coroutine, go));
			return go;
		}
	}
}

