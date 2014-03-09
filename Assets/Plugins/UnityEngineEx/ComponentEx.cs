using System;
using System.Collections.Generic;
using System.Reflection;
using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	public static class ComponentEx
	{
		#region Linkage

		public static T LinkSceneNodes<T>(this T c) where T : Component
		{
			return c.transform.LinkSceneNodes(c);
		}

		#endregion

		/// <summary>
		/// Sets SerializeFields of the Component to values form parameters object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="c"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static T Setup<T>(this T c, object parameters) where T : Component
		{
			var fields = new Dictionary<string, FieldInfo>();
			foreach (var field in c.GetType().GetFields<SerializeField>()) {
				fields.Add(field.Name, field);
			}
			foreach (var property in parameters.GetType().GetProperties()) {
				if (fields.ContainsKey(property.Name)) {
					var field = fields[property.Name];
					field.SetValue(c, property.GetValue(parameters, null));
				}
				else {
					Debug.LogWarning(String.Format("Property [{0}] not found int type [{1}]", property.Name, typeof(T).Name));
				}
			}

			return c;
		}

		/// <summary>
		/// Sets SerializeFields of the Component to values form parameters dictionary.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="c"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static T Setup<T>(this T c, IDictionary<string, object> parameters) where T : Component
		{
			foreach (var field in c.GetType().GetFields<SerializeField>()) {
				object value;
				if (parameters.TryGetValue(field.Name, out value)) {
					field.SetValue(c, value);
				}
			}

			return c;
		}

		/// <summary>
		/// Finds GameObject by path name. And returns it's Component T if it exists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="transform"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static T Find<T>(this Component c, string name) where T : Component
		{
			return c.transform.Find<T>(name);
		}

		public static GameObject FindGameObject(this Component c, string name)
		{
			return c.transform.FindGameObject(name);
		}

		public static Component SetActive(this Component c, bool flag)
		{
			c.gameObject.SetActive(flag);
			return c;
		}

		public static Bounds GetBounds(this Component c)
		{
			return c.gameObject.GetBounds();
		}
	}
}

