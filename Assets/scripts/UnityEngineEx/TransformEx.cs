using UnityEngine;


namespace UnityEngineEx
{
	public static class TransformEx
	{
		public static T Find<T>(this Transform transform, string name) where T : Component
		{
			var t = transform.Find(name);

			if (t != null)
				return t.gameObject.GetComponent<T>();

			return null;
		}
	}
}
