using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngineEx
{
	public static class GameObjectEx
	{
		public static GameObject Instantinate(this GameObject o, string name)
		{
			return o.Instantinate(name, Vector3.zero);
		}

		public static GameObject Instantinate(this GameObject o, string name, Vector3 po)
		{
			GameObject i = new GameObject(name);
			i.transform.parent = o.transform;
			i.transform.localPosition = po;
			return i;
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
			return c;
		}
	}
}
