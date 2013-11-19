using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngineEx
{
	public static class ComponentEx
	{
		public static IEnumerable<FieldInfo> GetFields<T>(this Component c)
		{
			foreach (var field in c.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)) {
				foreach (var attribute in field.GetCustomAttributes(true)) {
					if (attribute.GetType() == typeof(T)) {
						yield return field;
					}
				}
			}
			yield break;
		}

		public static T Setup<T>(this T c, object parameters) where T : Component
		{
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

