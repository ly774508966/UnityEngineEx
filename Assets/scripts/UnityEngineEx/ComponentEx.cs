using UnityEngine;
using System;
using SystemEx;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngineEx
{
	public static class ComponentEx
	{
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
					Debug.Log(String.Format("Property [{0}] not found int type [{1}]", property.Name, typeof(T).Name));
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
	}
}

