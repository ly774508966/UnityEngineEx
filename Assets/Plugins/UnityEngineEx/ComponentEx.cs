using System;
using System.Collections.Generic;
using System.Reflection;
using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	public static class ComponentEx
	{
		#region Dissolve

		public static T Dissolve<T>(this T c) where T : Component
		{
			return c.transform.Dissolve(c);
		}

		public static T Dissolve<T>(this T c, Action<T> i) where T : Component
		{
			c.gameObject.Dissolve(i);
			return c;
		}

		public static T Dissolve<T>(this T c, ActionContainer i) where T : Component
		{
			c.gameObject.Dissolve(i);
			return c;
		}

		public static T Dissolve<T>(this T c, params ActionContainer[] i) where T : Component
		{
			c.gameObject.Dissolve(i);
			return c;
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

		public static T AddComponent<T>(this Component c) where T : Component
		{
			return c.gameObject.AddComponent<T>();
		}

		public static T AddComponent<T>(this Component c, object parameters) where T : Component
		{
			return c.gameObject.AddComponent<T>(parameters);
		}

		/// <summary>
		/// Add a Component to the GameObject setting SerializeFields to a parameters values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="o"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static T AddComponent<T>(this Component c, IDictionary<string, object> parameters) where T : Component
		{
			return c.gameObject.AddComponent<T>(parameters);
		}

		/// <summary>
		/// Add a Component to the GameObject calling a ctor on it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="o"></param>
		/// <param name="ctor"></param>
		/// <returns></returns>
		public static T AddComponent<T>(this Component c, Action<T> ctor) where T : Component
		{
			return c.gameObject.AddComponent<T>(ctor);
		}

		/// <summary>
		/// Add a Component to the GameObject calling a ctor on it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="o"></param>
		/// <param name="ctor"></param>
		/// <returns></returns>
		public static T AddComponent<T>(this Component c, ActionContainer ctor) where T : Component
		{
			return c.gameObject.AddComponent<T>(ctor);
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

