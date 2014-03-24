using System;
using SystemEx;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngineEx
{
	public static class GameObjectEx
	{
		#region Enumerators

		/// <summary>
		/// Enumerates GameObject children.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static IEnumerable<GameObject> GetEnumerator(this GameObject o)
		{
			foreach (Transform child in o.transform)
				yield return child.gameObject;
			yield break;
		}

		/// <summary>
		/// Enumerates all GameObject children recursively.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static IEnumerable<GameObject> GetEnumeratorRecursive(this GameObject o)
		{
			Stack<Transform> next = new Stack<Transform>();

			next.Push(o.transform);
			while (next.Count != 0) {
				Transform current = next.Pop();
				foreach (Transform child in current) {
					yield return child.gameObject;
					next.Push(child);
				}
			}
			yield break;
		}

		/// <summary>
		/// Enumerates all GameObject children recursively preserving depth of recursion. 
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static IEnumerable<Tuple<GameObject, int>> GetEnumeratorRecursiveWithDepth(this GameObject o)
		{
			Stack<Tuple<Transform, int>> next = new Stack<Tuple<Transform, int>>();

			next.Push(Tuple.Create(o.transform, 0));
			while (next.Count != 0) {
				var current = next.Pop();
				foreach (Transform child in current.Item1) {
					yield return Tuple.Create(child.gameObject, current.Item2 + 1);
					next.Push(Tuple.Create(child, current.Item2 + 1));
				}
			}
			yield break;
		}

		#endregion

		#region Decompose

		public static GameObject Decompose(this GameObject o, ActionContainer i)
		{
			var prms = new object[i.args.Length];

			for (int ai = 0; ai < i.args.Length; ai++)
				prms[ai] = o.GetComponentOrThis(i.args[ai]);

			i.DynamicInvoke(prms);

			return o;
		}

		public static GameObject Decompose(this GameObject o, params ActionContainer[] i)
		{
			for (int ii = 0; ii < i.Length; ii++)
				o.Decompose(i[ii]);

			return o;
		}

		public static T Decompose<T>(this GameObject c, T o)
		{
			return c.transform.Decompose(o);
		}

		#endregion

		#region Creation

		/// <summary>
		/// Creates child object with a given name at given local position.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="name"></param>
		/// <param name="po"></param>
		/// <param name="components"></param>
		/// <returns></returns>
		public static GameObject Create(this GameObject o, string name, Vector3 po, params Type[] components)
		{
			GameObject i = new GameObject(name, components);
			i.transform.position = po;
			o.transform.Add(i);
			return i;
		}

		public static GameObject Create(this GameObject o, string name, params Type[] components)
		{
			return o.Create(name, Vector3.zero, components);
		}

		public static GameObject Create(this GameObject o, string name, ActionContainer ctor, params Type[] components)
		{
			var go = o.Create(name, Vector3.zero, components);
			go.Decompose(ctor);
			return go;
		}

		public static GameObject Create(this GameObject o, string name, Vector3 po, Mesh mesh, params Type[] components)
		{
			var go = o.Create(name, po, components);
			go.AddComponent<MeshRenderer>();
			go.AddComponent<MeshFilter>().mesh = mesh;
			return go;
		}

		public static GameObject Create(this GameObject o, string name, Mesh mesh, params Type[] components)
		{
			return o.Create(name, Vector3.zero, mesh, components);
		}

		public static GameObject Create(this GameObject o, string name, Vector3 po, Sprite sprite, params Type[] components)
		{
			var go = o.Create(name, po, components);
			go.AddComponent<SpriteRenderer>().sprite = sprite;
			return go;
		}

		public static GameObject Create(this GameObject o, string name, Sprite sprite, params Type[] components)
		{
			return o.Create(name, Vector3.zero, sprite, components);
		}

		public static GameObject Create(this GameObject o, string name, Vector3 po, string text, params Type[] components)
		{
			var go = o.Create(name, po, components);
			go.AddComponent<TextMesh>().text = text;
			return go;
		}

		public static GameObject Create(this GameObject o, string name, string text, params Type[] components)
		{
			return o.Create(name, Vector3.zero, text, components);
		}

		#endregion

		#region Instantiation

		public static GameObject New(this GameObject instance, params Tuple<Type, object>[] initializers)
		{
			GameObject go = null;
			bool a = instance.activeSelf;
			instance.SetActive(false);

			try {
				go = GameObject.Instantiate(instance) as GameObject;

				foreach (var i in initializers) try {
					var c = go.GetComponent(i.Item1);
					if (c != null)
						c.Setup(i.Item2);
				} catch (Exception e) { Debug.LogException(e); }

				go.SetActive(a);
			} catch (Exception e) { Debug.LogException(e); }
			
			instance.SetActive(a);
			return go;
		}

		public static GameObject New(this GameObject instance, params Tuple<Type, IDictionary<string, object>>[] initializers)
		{
			GameObject go = null;
			bool a = instance.activeSelf;
			instance.SetActive(false);

			try {
				go = GameObject.Instantiate(instance) as GameObject;

				foreach (var i in initializers) try {
					var c = go.GetComponent(i.Item1);
					if (c != null)
						c.Setup(i.Item2);
				} catch (Exception e) { Debug.LogException(e); }

				go.SetActive(a);
			} catch (Exception e) { Debug.LogException(e); }

			instance.SetActive(a);
			return go;
		}

		public static GameObject New(this GameObject instance, params ActionContainer[] initializers)
		{
			GameObject go = null;
			bool a = instance.activeSelf;
			instance.SetActive(false);

			try {
				go = GameObject.Instantiate(instance) as GameObject;

				foreach (var i in initializers) try {
					go.Decompose(i);
				} catch (Exception e) { Debug.LogException(e); }

				go.SetActive(a);			
			} catch (Exception e) { Debug.LogException(e); }

			instance.SetActive(a);
			return go;
		}

		public static GameObject New(this GameObject instance, string name, Vector3 po, params Tuple<Type, object>[] initializers)
		{
			GameObject go = null;
			bool a = instance.activeSelf;
			instance.SetActive(false);

			try {
				go = GameObject.Instantiate(instance) as GameObject;

				if (initializers != null) {
					foreach (var i in initializers) try {
						var c = go.GetComponent(i.Item1);
						if (c != null)
							c.Setup(i.Item2);
					} catch (Exception e) { Debug.LogException(e); }
				}

				go.transform.position = po;
				go.SetActive(a);
			} catch (Exception e) { Debug.LogException(e); }
			
			instance.SetActive(a);
			return go;
		}

		//[Obsolete("Instantiate semantics have changed.")]
		public static GameObject Instantiate(GameObject instance, params Tuple<Type, Action<object>>[] initializers)
		{
			GameObject go = null;
			bool a = instance.activeSelf;
			instance.SetActive(false);

			try {
				go = GameObject.Instantiate(instance) as GameObject;

				foreach (var i in initializers) try {
					var c = go.GetComponent(i.Item1);
					if (c != null)
						i.Item2(c);
				} catch (Exception e) { Debug.LogException(e); }

				go.SetActive(a);
			} catch (Exception e) { Debug.LogException(e); }

			instance.SetActive(a);
			return go;
		}

		//[Obsolete("Instantiate semantics have changed.")]
		public static GameObject Instantiate(this GameObject o, GameObject instance, Vector3 po, params Tuple<Type, object>[] initializers)
		{
			GameObject go = null;
			bool a = instance.activeSelf;
			instance.SetActive(false);

			try {
				go = GameObject.Instantiate(instance) as GameObject;

				if (initializers != null) {
					foreach (var i in initializers) try {
						var c = go.GetComponent(i.Item1);
						if (c != null)
							c.Setup(i.Item2);
					} catch (Exception e) { Debug.LogException(e); }
				}

				go.transform.position = po;
				o.transform.Add(go);
				go.SetActive(a);
			} catch (Exception e) { Debug.LogException(e); }
			
			instance.SetActive(a);
			return go;
		}

		//[Obsolete("Instantiate semantics have changed.")]
		public static GameObject Instantiate(this GameObject o, GameObject instance, params Tuple<Type, object>[] initializers)
		{
			return o.Instantiate(instance, Vector3.zero, initializers);
		}

		/// <summary>
		/// Instantiate new GameObject in place of GameObject.
		/// Function fully replaces one object by another.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="instance"></param>
		/// <param name="initializers"></param>
		/// <returns></returns>
		public static GameObject Reinstantiate(this GameObject o, GameObject instance, params Tuple<Type, object>[] initializers)
		{
			GameObject go = null;
			bool a = instance.activeSelf;
			instance.SetActive(false);

			try {
				go = GameObject.Instantiate(instance) as GameObject;

				foreach (var i in initializers) try {
					var c = go.GetComponent(i.Item1);
					if (c != null)
						c.Setup(i.Item2);
				} catch (Exception e) { Debug.LogException(e); }

				go.name = o.name;
				go.transform.position = o.transform.localPosition + go.transform.position;
				go.transform.rotation = o.transform.localRotation * go.transform.rotation;
				go.transform.localScale = o.transform.localScale;
				o.transform.parent.Add(go);
				GameObject.DestroyImmediate(o);
				go.SetActive(a);				
			} catch (Exception e) { Debug.LogException(e); }

			instance.SetActive(a);
			return go;
		}

#if MONO_BUG_IS_FIXED
		public static GameObject Reinstantiate(this GameObject o, GameObject instance, params object[] initializers)
#else
		public static GameObject Reinstantiate2(this GameObject o, GameObject instance, params object[] initializers)
#endif
		{
			GameObject go = null;
			bool a = instance.activeSelf;
			instance.SetActive(false);
			
			try {
				go = GameObject.Instantiate(instance) as GameObject;

				foreach (var i in initializers) try {
					Type ct = i.GetType().GetGenericArguments()[0];
					var c = go.GetComponentOrThis(ct);
					if (c != null)
						((Delegate)(i)).DynamicInvoke(c);
				} catch (Exception e) { Debug.LogException(e); }

				go.name = o.name;
				go.transform.position = o.transform.localPosition + go.transform.position;
				go.transform.rotation = o.transform.localRotation * go.transform.rotation;
				go.transform.localScale = o.transform.localScale;
				o.transform.parent.Add(go);
				GameObject.DestroyImmediate(o);
				go.SetActive(a);				
			} catch (Exception e) { Debug.LogException(e); }
			
			instance.SetActive(a);
			return go;
		}
				
		#endregion

#if !UNITY_EDITOR
		public static void Destroy(GameObject o)
		{
			GameObject.Destroy(o);
		}
#else
		public static void Destroy(GameObject o)
		{
			if (UnityEditor.EditorApplication.isPlaying)
				GameObject.Destroy(o);
			else
				GameObject.DestroyImmediate(o);
		}
#endif

		/// <summary>
		/// Add a Component to the GameObject setting SerializeFields to a parameters values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="o"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static T AddComponent<T>(this GameObject o, object parameters) where T : Component
		{
			bool a = o.activeSelf;
			o.SetActive(false);

			T c = o.AddComponent<T>().Setup(parameters);

			o.SetActive(a);
			return c;
		}

		/// <summary>
		/// Add a Component to the GameObject setting SerializeFields to a parameters values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="o"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static T AddComponent<T>(this GameObject o, IDictionary<string, object> parameters) where T : Component
		{
			bool a = o.activeSelf;
			o.SetActive(false);

			T c = o.AddComponent<T>().Setup(parameters);

			o.SetActive(a);
			return c;
		}

		/// <summary>
		/// Add a Component to the GameObject calling a ctor on it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="o"></param>
		/// <param name="ctor"></param>
		/// <returns></returns>
		public static T AddComponent<T>(this GameObject o, Action<T> ctor) where T : Component
		{
			bool a = o.activeSelf;
			o.SetActive(false);

			T c = o.AddComponent<T>();
			ctor(c);

			o.SetActive(a);
			return c;
		}

		/// <summary>
		/// Add a Component to the GameObject calling a ctor on it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="o"></param>
		/// <param name="ctor"></param>
		/// <returns></returns>
		public static T AddComponent<T>(this GameObject o, ActionContainer ctor) where T : Component
		{
			bool a = o.activeSelf;
			o.SetActive(false);

			T c = o.AddComponent<T>();

			object[] prms = new object[ctor.args.Length];
			for (int ai = 0; ai < ctor.args.Length; ai++) {
				if (ctor.args[ai] != typeof(T))
					prms[ai] = o.GetComponentOrThis(ctor.args[ai]);
				else
					prms[ai] = c;
			}

			ctor.DynamicInvoke(prms);

			o.SetActive(a);
			return c;
		}

		public static object GetComponentOrThis(this GameObject o, Type type)
		{
			if (type != typeof(GameObject))
				return o.GetComponent(type);
			else
				return o;
		}

		/// <summary>
		/// Adds GameObject as a child to another GameObject.
		/// Objects position and rotation are set to localPosition and localrotation.
		/// <seealso cref="TransformEx.Add"/>
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public static GameObject Add(this GameObject parent, GameObject o)
		{
			return parent.transform.Add(o).gameObject;
		}

		public static GameObject SetParent(this GameObject o, GameObject parent)
		{
			parent.transform.Add(o);
			return o;
		}
		

		/// <summary>
		/// Get most accurate Bounds of the GameObject.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static Bounds GetBounds(this GameObject o)
		{
			Bounds b = BoundsEx.Empty;
			o.CallRecursive((GameObject c) => {
				if (!c.activeInHierarchy)
					return;

				var renderer = c.GetComponent<Renderer>();
				if (renderer != null) {
					b = b.Extend(renderer.bounds);
				}
			});

			return b;
		}

		/// <summary>
		/// Get most accurate Bounds of the GameObject.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static Bounds GetBoundsLocal(this GameObject o)
		{
			var renderer = o.GetComponent<Renderer>();
			if (renderer != null) {
				return renderer.bounds;
			}

			return new Bounds();
		}

		/// <summary>
		/// Apply some Action recursively to GameObject and all its child objects.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static GameObject CallRecursive(this GameObject o, Action<GameObject> a)
		{
			a(o);
			foreach (GameObject child in o.GetEnumeratorRecursive()) {
				a(child);
			}

			return o;
		}

		/// <summary>
		/// Apply some Action recursively to GameObject and all its child objects passing depth as a parameter to action.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static GameObject CallRecursive(this GameObject o, Action<GameObject, int> a)
		{
			a(o, 0);
			foreach (var child in o.GetEnumeratorRecursiveWithDepth()) {
				a(child.Item1, child.Item2);
			}

			return o;
		}
	}
}
