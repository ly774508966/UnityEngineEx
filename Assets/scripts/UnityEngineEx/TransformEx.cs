using System;
using System.Collections;
using System.Collections.Generic;
using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	public static class TransformEx
	{
		/// <summary>
		/// Adds GameObject as a child to a transform.
		/// Objects position and rotation are set to localPosition and localrotation.
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public static Transform Add(this Transform transform, GameObject o)
		{
			var po = o.transform.position;
			var ro = o.transform.rotation;
			var sc = o.transform.localScale;
			o.transform.parent = transform;
			o.transform.localPosition = po;
			o.transform.localRotation = ro;
			o.transform.localScale = sc;
			return transform;
		}
		
		/// <summary>
		/// Removes all child GameObjects.
		/// </summary>
		/// <param name="transform"></param>
		/// <returns></returns>
		public static Transform Clear(this Transform transform)
		{
			foreach (Transform child in transform) {
				GameObjectEx.Destroy(child.gameObject);
			}

			return transform;
		}

		/// <summary>
		/// Removes all child GameObjects.
		/// </summary>
		/// <param name="transform"></param>
		/// <returns></returns>
		public static Transform Clear(this Transform transform, Func<Transform, bool> f)
		{
			foreach (Transform child in transform.Find(f)) {
				GameObjectEx.Destroy(child.gameObject);
			}

			return transform;
		}

		/// <summary>
		/// Unlinks all child GameObjects.
		/// </summary>
		/// <param name="transform"></param>
		/// <returns></returns>
		public static Transform Unlink(this Transform transform)
		{
			foreach (Transform child in transform) {
				child.parent = null;
			}

			return transform;
		}

		public static Transform SetActive(this Transform transform, bool flag)
		{
			transform.gameObject.SetActive(flag);
			return transform;
		}

		/// <summary>
		/// Finds GameObject by path name. And returns it's Component T if it exists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="transform"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static T Find<T>(this Transform transform, string name) where T : Component
		{
			var t = transform.Find(name);

			if (t != null)
				return t.gameObject.GetComponent<T>();
			else
				Debug.Log(string.Format("No child GameObject '{0}' found.", name));

			return null;
		}

		public static object Find(this Transform transform, string name, Type type)
		{
			var t = name != null ? transform.Find(name) : transform;

			if (t != null)
				return t.gameObject.GetComponentOrThis(type);
			else
				Debug.Log(string.Format("No child GameObject '{0}' found.", name));

			return null;
		}

		public static IEnumerable<Transform> Find(this Transform transform, Func<Transform, bool> f)
		{
			foreach (Transform child in transform) {
				if (f(child))
					yield return child;
			}

			yield break;
		}

		#region Linkage

		public static T LinkSceneNodes<T>(this Transform transform, T o)
		{
			foreach (var field in o.GetType().GetFieldsAndAttributes<LinkToSceneAttribute>()) {
				if (field.Item1.FieldType.IsSubclassOf(typeof(UnityEngine.Object))) {
					field.Item1.SetValue(o, transform.Find(field.Item2.name, field.Item1.FieldType));
				}
				else {
					if (field.Item1.FieldType.IsGenericType && field.Item1.FieldType.GetGenericTypeDefinition() == typeof(IList<>)) {
						Type nodeType = field.Item1.FieldType.GetGenericArguments()[0];
						IList list = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(nodeType));

						foreach (Transform child in transform.Find(field.Item2.name)) {
							list.Add(child.gameObject.GetComponentOrThis(nodeType));
						}

						field.Item1.SetValue(o, list);
					}
				}
			}

			return o;
		}

		#endregion
	}
}
