﻿using UnityEngine;
using System;

namespace UnityEngineEx
{
	public static class GameObjectEx
	{
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

		public static GameObject Instantiate(GameObject instance, params Tuple<Type, object>[] initializers)
		{
			bool a = instance.activeSelf;
			instance.SetActive(false);

			var go = GameObject.Instantiate(instance) as GameObject;

			foreach (var i in initializers) {
				var c = go.GetComponent(i.Item1);
				if (c != null)
					c.Setup(i.Item2);
			}

			instance.SetActive(a);
			go.SetActive(a);
			return go;
		}

		public static GameObject Instantiate(this GameObject o, GameObject instance, params Tuple<Type, object>[] initializers)
		{
			bool a = instance.activeSelf;
			instance.SetActive(false);

			var go = GameObject.Instantiate(instance) as GameObject;

			foreach (var i in initializers) {
				var c = go.GetComponent(i.Item1);
				if (c != null)
					c.Setup(i.Item2);
			}

			o.transform.Add(go);

			instance.SetActive(a);
			go.SetActive(a);
			return go;
		}

		public static GameObject Reinstantiate(this GameObject o, GameObject instance, params Tuple<Type, object>[] initializers)
		{
			bool a = instance.activeSelf;
			instance.SetActive(false);

			var go = GameObject.Instantiate(instance) as GameObject;

			foreach (var i in initializers) {
				var c = go.GetComponent(i.Item1);
				if (c != null)
					c.Setup(i.Item2);
			}

			go.name = o.name;
			go.transform.position = o.transform.localPosition + go.transform.position;
			go.transform.rotation = o.transform.localRotation * go.transform.rotation;
			o.transform.parent.Add(go);
			GameObject.Destroy(o);

			instance.SetActive(a);
			go.SetActive(a);
			return go;
		}

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
		/// Get most accurate Bounds of the GameObject. Trying to get bounds of MeshFilter, SpriteRenderer and Renderer components in that order.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static Bounds GetBounds(this GameObject o)
		{
			var mesh = o.GetComponent<MeshFilter>();
			if (mesh != null && mesh.sharedMesh != null) {
				return mesh.sharedMesh.bounds;
			}

			var sprite = o.GetComponent<SpriteRenderer>();
			if (sprite != null && sprite.sprite != null) {
				return sprite.sprite.bounds;
			}

			var renderer = o.GetComponent<Renderer>();
			if (renderer != null) {
				return renderer.bounds;
			}

			return new Bounds();
		}
	}
}
