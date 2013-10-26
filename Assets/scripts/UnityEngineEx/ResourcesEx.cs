using UnityEngine;
using System;

namespace UnityEngineEx
{
	public static class ResourcesEx
	{
		public static T Load<T>(string path) where T : class
		{
			if (path == ".color") {
				return LoadColor<T>();
			}
			return Resources.Load(path, typeof(T)) as T;
		}

		static Material color_;
		static T LoadColor<T>() where T : class
		{
			if (typeof(T) == typeof(Material)) {
				if (color_ == null) {
					color_ = new Material("Shader \".color\" { Properties {} SubShader { Pass { ColorMaterial AmbientAndDiffuse } } }");
				}
				return color_ as T;
			}
			return null;
		}
	}
}

