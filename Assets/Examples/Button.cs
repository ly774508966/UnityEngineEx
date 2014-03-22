using UnityEngine;
using UnityEngineEx;
using System;
using SystemEx;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]
public class Button : MonoBehaviour
{
	public Action OnCommand;

	void Start()
	{
		this.Decompose(_.a((MeshCollider mc, MeshFilter mf) => {
			if (mf != null) {
				mc.sharedMesh = mf.sharedMesh;
			}
		}));
	}
}
