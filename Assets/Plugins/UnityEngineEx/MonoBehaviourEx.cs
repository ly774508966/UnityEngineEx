using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	public class MonoBehaviourEx : MonoBehaviour
	{
		class MonoBehaviourDescription 
		{
			public List<MethodInfo> awake = new List<MethodInfo>(); 
		}

		static Dictionary<Type, MonoBehaviourDescription> descriptions = new Dictionary<Type,MonoBehaviourDescription>();

		protected virtual void Awake()
		{
			this.Dissolve();

			MonoBehaviourDescription mbd;
			if (!descriptions.TryGetValue(this.GetType(), out mbd)) {
				mbd = new MonoBehaviourDescription();
				foreach (var m in this.GetType().GetMethodsAndAttributes<BehaviourFunctionAttribute>()) {
					if (m.Item2.name == "Awake")
						mbd.awake.Add(m.Item1);
				}

				descriptions.Add(this.GetType(), mbd);
			}

			foreach (var m in mbd.awake) {
				var mp = m.GetParameters();
				if (mp.Length > 0) {
					var pl = new List<object>(mp.Length);
					foreach (var p in mp) {
						pl.Add(gameObject.GetComponentOrAdd(p.ParameterType));
					}
					m.Invoke(this, pl.ToArray());
				}
			}
		}

		protected virtual void Start()
		{

		}

		protected virtual void Update()
		{

		}
	}
}
