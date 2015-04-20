using System;
using System.Collections.Generic;
using System.Reflection;
using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	public class MonoBehaviourEx : MonoBehaviour
	{
		private class MonoBehaviourDescription
		{
			public List<FieldInfo> addComponents = new List<FieldInfo>();

			public List<MethodInfo> awake = new List<MethodInfo>();
			public List<MethodInfo> start = new List<MethodInfo>();
			public List<MethodInfo> update = new List<MethodInfo>();
		}

		private static Dictionary<Type, MonoBehaviourDescription> descriptions = new Dictionary<Type, MonoBehaviourDescription>();
		private MonoBehaviourDescription mbd;

		protected virtual void Awake()
		{
			this.Dissolve();

			if (!descriptions.TryGetValue(this.GetType(), out mbd)) {
				mbd = new MonoBehaviourDescription();

				foreach (var m in this.GetType().GetMethodsAndAttributes<BehaviourFunctionAttribute>()) {
					if (m.Item2.name == "Awake")
						mbd.awake.Add(m.Item1);
					if (m.Item2.name == "Start")
						mbd.start.Add(m.Item1);
					if (m.Item2.name == "Update")
						mbd.update.Add(m.Item1);
				}

				foreach (var m in this.GetType().GetFieldsAndAttributes<AddComponentAttribute>()) {
					mbd.addComponents.Add(m.Item1);
				}

				descriptions.Add(this.GetType(), mbd);
			}

			foreach (var ac in mbd.addComponents) {
				ac.SetValue(this, gameObject.AddComponent(ac.FieldType));
			}

			foreach (var m in mbd.awake) {
				var mp = m.GetParameters();
				if (mp.Length > 0) {
					var pl = new List<object>(mp.Length);
					foreach (var p in mp) {
						pl.Add(gameObject.GetComponent(p.ParameterType));
					}
					m.Invoke(this, pl.ToArray());
				}
				else {
					m.Invoke(this, null);
				}
			}
		}

		protected virtual void Start()
		{
			if (mbd == null) {
				Log.Error("MonoBehaviourDescription is not found, possibly Awake was not properly overrided.");
			}

			if (mbd.start.Count == 0)
				return;

			foreach (var m in mbd.start) {
				var mp = m.GetParameters();
				if (mp.Length > 0) {
					var pl = new List<object>(mp.Length);
					foreach (var p in mp) {
						pl.Add(gameObject.GetComponent(p.ParameterType));
					}
					m.Invoke(this, pl.ToArray());
				}
				else {
					m.Invoke(this, null);
				}
			}
		}

		protected virtual void Update()
		{
			if (mbd == null) {
				Log.Error("MonoBehaviourDescription is not found, possibly Awake was not properly overrided.");
			}

			if (mbd.update.Count == 0)
				return;

			foreach (var m in mbd.update) {
				var mp = m.GetParameters();
				if (mp.Length > 0) {
					var pl = new List<object>(mp.Length);
					foreach (var p in mp) {
						pl.Add(gameObject.GetComponent(p.ParameterType));
					}
					m.Invoke(this, pl.ToArray());
				}
				else {
					m.Invoke(this, null);
				}
			}
		}
	}
}