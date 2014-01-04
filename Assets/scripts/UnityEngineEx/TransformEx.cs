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
			o.transform.parent = transform;
			o.transform.localPosition = po;
			o.transform.localRotation = ro;
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
				GameObject.Destroy(child.gameObject);
			}

			return transform;
		}

#if UNITY_EDITOR
		/// <summary>
		/// Removes all child GameObjects.
		/// </summary>
		/// <param name="transform"></param>
		/// <returns></returns>
		public static Transform ClearImmidiate(this Transform transform)
		{
			foreach (Transform child in transform) {
				GameObject.DestroyImmediate(child.gameObject);
			}

			return transform;
		}
#endif

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
	}
}
