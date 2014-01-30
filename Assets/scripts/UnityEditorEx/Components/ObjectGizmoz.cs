using UnityEngine;
using UnityEngineEx;
using System;
using SystemEx;
using System.Collections.Generic;


namespace UnityEditorEx
{
	public class ObjectGizmoz : MonoBehaviour
	{
		void Awake()
		{
		}

		void OnDrawGizmos()
		{
			if (renderer != null) {
				Gizmos.color = Color.green;
				Gizmos.DrawWireCube(renderer.bounds.center, renderer.bounds.size);
			}
		}
	}
}
