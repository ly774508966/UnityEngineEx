using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	public static class PlayerPrefsEx
	{
		public static Vector3 GetVector3(string key)
		{
 			string v = PlayerPrefs.GetString(key);
			float[] tokens = ArrayEx.Parse<float>(v);
			return new Vector3(tokens[0], tokens[1], tokens[2]);
		}

		public static void SetVector3(string key, Vector3 v)
		{
			string[] a = new string[3] { v.x.ToString(), v.y.ToString(), v.z.ToString() };
			PlayerPrefs.SetString(key, string.Join(":", a));
		}
	}
}

