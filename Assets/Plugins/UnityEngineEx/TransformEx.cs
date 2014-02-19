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
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlaying) {
				List<GameObject> objects = new List<GameObject>();
				foreach (Transform child in transform) {
					objects.Add(child.gameObject);
				}
				foreach (GameObject o in objects) {
					GameObjectEx.Destroy(o);
				}
			}
			else
#endif
			{
				foreach (Transform child in transform) {
					GameObjectEx.Destroy(child.gameObject);
				}
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
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlaying) {
				List<GameObject> objects = new List<GameObject>();
				foreach (Transform child in transform.Find(f)) {
					objects.Add(child.gameObject);
				}
				foreach (GameObject o in objects) {
					GameObjectEx.Destroy(o);
				}
			}
			else
#endif
			{
				foreach (Transform child in transform.Find(f)) {
					GameObjectEx.Destroy(child.gameObject);
				}
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
				Debug.LogWarning(string.Format("No child GameObject '{0}' found.", name));

			return null;
		}

		public static object Find(this Transform transform, string name, Type type)
		{
			var t = name != null ? transform.Find(name) : transform;

			if (t != null)
				return t.gameObject.GetComponentOrThis(type);
			else
				Debug.LogWarning(string.Format("No child GameObject '{0}' found.", name));

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
						if (!nodeType.IsVisible) Debug.LogError(nodeType.FullName + " should be declared public or it will break Mono builds.");

						IList list = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(nodeType));
								
						foreach (Transform child in transform.Find(field.Item2.name)) {
							if (nodeType.IsSubclassOf(typeof(UnityEngine.Object))) {
								list.Add(child.gameObject.GetComponentOrThis(nodeType));
							}
							else {
								object node = Activator.CreateInstance(nodeType);								
								list.Add(child.LinkSceneNodes(node));																
							}
						}

						field.Item1.SetValue(o, list);
					}
					else {
						object node = Activator.CreateInstance(field.Item1.FieldType);
						field.Item1.SetValue(o, transform.LinkSceneNodes(node));
					}
				}
			}

			return o;
		}

		#endregion
	}
}
