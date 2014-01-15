using System;
using System.Collections.Generic;
using System.Reflection;

namespace SystemEx
{
	public static class TypeEx
	{
		public static bool HaveAttribute<A>(this MemberInfo mi) where A : Attribute
		{
			foreach (var attribute in mi.GetCustomAttributes(true)) {
				if (attribute.GetType() == typeof(A)) {
					return true;
				}
			}

			return false;
		}

		public static A GetAttribute<A>(this MemberInfo mi) where A : Attribute
		{
			foreach (var attribute in mi.GetCustomAttributes(true)) {
				if (attribute.GetType() == typeof(A)) {
					return (A)attribute;
				}
			}

			return null;
		}

		/// <summary>
		/// Lists all private fields with attribute A.
		/// </summary>
		/// <typeparam name="A"></typeparam>
		/// <param name="t"></param>
		/// <returns></returns>
		public static IEnumerable<FieldInfo> GetFields<A>(this Type t) where A : Attribute
		{
			foreach (var field in t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)) {
				if (field.HaveAttribute<A>())
					yield return field;
			}
			yield break;
		}

		/// <summary>
		/// Lists all private methods with attribute A.
		/// </summary>
		/// <typeparam name="A"></typeparam>
		/// <param name="t"></param>
		/// <returns></returns>
		public static IEnumerable<MethodInfo> GetMethods<A>(this Type t) where A : Attribute
		{
			foreach (var method in t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)) {
				if (method.HaveAttribute<A>())
					yield return method;
			}
			yield break;
		}
	}
}
