using UnityEngine;
using System;

namespace UnityEngineEx
{
	public static class GameObjectEx
	{
		public static GameObject Instantiate(this GameObject o, string name, params Type[] components)
		{
			return o.Instantiate(name, Vector3.zero, components);
		}

		public static GameObject Instantiate(this GameObject o, string name, Vector3 po, params Type[] components)
		{
			GameObject i = new GameObject(name, components);
			i.transform.parent = o.transform;
			i.transform.localPosition = po;
			return i;
		}

		public static GameObject Instantiate(this GameObject o, string name, Mesh mesh, params Type[] components)
		{
			return o.Instantiate(name, Vector3.zero, mesh, components);
		}

		public static GameObject Instantiate(this GameObject o, string name, Vector3 po, Mesh mesh, params Type[] components)
		{
			var go = o.Instantiate(name, po, components);
			go.AddComponent<MeshRenderer>();
			go.AddComponent<MeshFilter>().mesh = mesh;
			return go;
		}

		public static GameObject Instantiate(this GameObject o, GameObject instance, params Tuple<Type, object>[] initializers)
		{
			bool a = instance.activeSelf;
			instance.SetActive(false);

			var go = GameObject.Instantiate(instance) as GameObject;

			foreach (var i in initializers) {
				var c = go.GetComponent(i.Item1);
				if (c != null)
					c.Setup(i.Item2);
			}

			instance.SetActive(a);
			go.SetActive(a);
			return o;
		}


		public static GameObject Add(this GameObject o, GameObject item)
		{
			Vector3 po = item.transform.position;
			item.transform.parent = o.transform;
			item.transform.localPosition = po;
			return o;
		}

		public static T AddComponent<T>(this GameObject o, object parameters) where T : Component
		{
			bool a = o.activeSelf;
			o.SetActive(false);

			T c = o.AddComponent<T>().Setup(parameters);

			o.SetActive(a);
			return c;
		}
	}
}
