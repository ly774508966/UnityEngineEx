using System;
using System.Collections;
using System.Collections.Generic;
using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	public class MonoBehaviourEx : MonoBehaviour
	{
		protected virtual void Awake()
		{
			this.Dissolve();

			foreach (var m in this.GetType().GetMethods("Awake_")) {
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
