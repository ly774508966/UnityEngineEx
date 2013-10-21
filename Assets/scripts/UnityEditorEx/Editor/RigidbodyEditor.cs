using UnityEngine;
using UnityEditor;
using System.Threading;
using System.Collections.Generic;
 
[CustomEditor(typeof(Rigidbody))]
public class RigidbodyEditor : Editor
{
	struct Value<T> {
		public T __;
		public T _;


		public static bool operator ~(Value<T> v)
		{
			return !EqualityComparer<T>.Default.Equals(v._, v.__);
		}

		public static T operator +(Value<T> v)
		{
			return v._;
		}

		public static Value<T> operator /(Value<T> l, T r)
		{
			l.__ = l._;
			l._ = r;
			return l;
		}
	}

	class Data {
		public Value<bool> enable_ = new Value<bool>();

		static Data instance_;
		public static Data instance {
			get {
				if (instance_ == null) {
					instance_ = new Data();
				}
				return instance_;
			}
		}
	}

	Data _ { get { return Data.instance; } }




    public override void OnInspectorGUI()
    {
        // ????????? ????????? ?????????? ?? ?????????
        DrawDefaultInspector();

		Component c = target as Component;
		if (!c)
			return;

		GUILayout.Label(string.Format("Curve length: {0}", 666));
		_.enable_ /= GUILayout.Toggle(+_.enable_, "Enable?");

		if (~_.enable_) {
			if (+_.enable_) {
				c.gameObject.AddComponent<Bezier>();
			} else {
				DestroyImmediate(c.gameObject.GetComponent<Bezier>());
			}
		}
    }
 
    // ????????? ? ?????, ????? ? ??????? ?? ??????????, ??? ?? ????????????
    // ??? ????????? ????? Gizmos ???????????? ???? Handles (????????????)
    public void OnSceneGUI()
    {
		Rigidbody rigidbody = target as Rigidbody;

		if (rigidbody) {
			Quaternion rot = Quaternion.identity;
			float size = HandleUtility.GetHandleSize(rigidbody.transform.position + new Vector3(0, 1, 0)) * 0.2f;
			Handles.FreeMoveHandle(rigidbody.transform.position + new Vector3(0, 1, 0), rot, size, Vector3.zero, Handles.SphereCap);
		}

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
 
}
