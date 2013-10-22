using UnityEngine;
using System;

namespace UnityEngineEx
{
	public static class ResourcesEx
	{
		public static T Load<T>(string path) where T : class
		{
			return Resources.Load(path, typeof(T)) as T;
		}
	}
}

