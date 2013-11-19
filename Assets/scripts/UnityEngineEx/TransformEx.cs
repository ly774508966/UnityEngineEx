using UnityEngine;


namespace UnityEngineEx
{
	public static class TransformEx
	{
		public static Transform Add(this Transform transform, GameObject o)
		{
			var po = o.transform.position;
			var ro = o.transform.rotation;
			o.transform.parent = transform;
			o.transform.localPosition = po;
			o.transform.localRotation = ro;
			return transform;
		}



		public static Transform Clear(this Transform transform)
		{
			foreach (Transform child in transform) {
				GameObject.Destroy(child.gameObject);
			}

			return transform;
		}

		public static T Find<T>(this Transform transform, string name) where T : Component
		{
			var t = transform.Find(name);

			if (t != null)
				return t.gameObject.GetComponent<T>();

			return null;
		}
	}
}
