using System;
using System.Collections.Generic;
using System.Reflection;

namespace SystemEx
{
	public static class TypeEx
	{
		/// <summary>
		/// Lists all private fields with attribute A.
		/// </summary>
		/// <typeparam name="A"></typeparam>
		/// <param name="c"></param>
		/// <returns></returns>
		public static IEnumerable<FieldInfo> GetFields<A>(this Type c) where A : Attribute
		{
			foreach (var field in c.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)) {
				foreach (var attribute in field.GetCustomAttributes(true)) {
					if (attribute.GetType() == typeof(A)) {
						yield return field;
					}
				}
			}
			yield break;
		}

		/// <summary>
		/// Lists all private methods with attribute A.
		/// </summary>
		/// <typeparam name="A"></typeparam>
		/// <param name="c"></param>
		/// <returns></returns>
		public static IEnumerable<MethodInfo> GetMethods<A>(this Type c) where A : Attribute
		{
			foreach (var method in c.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)) {
				foreach (var attribute in method.GetCustomAttributes(true)) {
					if (attribute.GetType() == typeof(A)) {
						yield return method;
					}
				}
			}
			yield break;
		}
	}
}
