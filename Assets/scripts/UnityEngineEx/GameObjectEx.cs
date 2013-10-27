using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

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

			var c = o.AddComponent<T>();

			var fields = new Dictionary<string, FieldInfo>();
			foreach (var field in c.GetFields<SerializeField>()) {
				fields.Add(field.Name, field);
			}
			foreach (var property in parameters.GetType().GetProperties()) {
				if (fields.ContainsKey(property.Name)) {
					var field = fields[property.Name];
					field.SetValue(c, property.GetValue(parameters, null));
				}
			}
			var awakeEx = typeof(T).GetMethod("AwakeEx", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			if (awakeEx != null) {
				awakeEx.Invoke(c, null);
			}

			o.SetActive(a);
			return c;
		}
	}
}
