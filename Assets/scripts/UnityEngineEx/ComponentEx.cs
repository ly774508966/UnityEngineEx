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
	}
}

