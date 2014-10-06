using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace UnityEngineEx
{
	public static class ResourcesEx
	{
		public static readonly string projectPath;
		public static readonly string resourcePath;

		static ResourcesEx()
		{
			string codeBase = Assembly.GetExecutingAssembly().CodeBase;
			UriBuilder uri = new UriBuilder(codeBase);
			string path = Uri.UnescapeDataString(uri.Path);
			projectPath = Path.GetFullPath(new Uri(Path.Combine(Path.GetDirectoryName(path), "../..")).AbsolutePath);
			resourcePath = Path.GetFullPath(new Uri(Path.Combine(Path.GetDirectoryName(path), "../../Assets/Resources")).AbsolutePath);
		}

		public static string GetAssetPath(string path)
		{
			var p = Path.GetFullPath(path);
			if (p.StartsWith(projectPath)) {
				return p.Substring(projectPath.Length + 1);
			}

			return null;
		}

		public static string GetResourcePath(string path)
		{
			var p = Path.GetDirectoryName(path);
			if (p.StartsWith(resourcePath)) {
				return p.Substring(resourcePath.Length + 1) + "/" + Path.GetFileNameWithoutExtension(path);
			}
			if (p.StartsWith("Assets/Resources")) {
				return p.Substring("Assets/Resources".Length + 1) + "/" + Path.GetFileNameWithoutExtension(path);
			}

			return null;
		}

		public static T Load<T>(string path) where T : class
		{
			if (path == ".color") {
				return LoadColor<T>();
			}
			return Resources.Load(path, typeof(T)) as T;
		}

		private static Material color_;

		private static T LoadColor<T>() where T : class
		{
			if (typeof(T) == typeof(Material)) {
				if (color_ == null) {
					color_ = new Material("Shader \".color\" { SubShader { Pass { ColorMaterial AmbientAndDiffuse } } }");
				}
				return color_ as T;
			}
			return null;
		}
	}
}