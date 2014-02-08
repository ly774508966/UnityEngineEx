using System;

namespace SystemEx
{
	public static class ArrayEx
	{
		public static bool Contains<T>(this T[] array, T value)
		{
			return Array.IndexOf(array, value) != -1;
		}

		public static T[] ForEach<T>(this T[] array, Action<T> action)
		{
			Array.ForEach(array, action);
			return array;
		}

		public static T[] Parse<T>(string value)
		{
			string[] tokens = value.Split(new Char[] { ':' });
			T[] result = new T[tokens.Length];

			for (int i = 0; i < result.Length; i++) {
				result[i] = (T)Convert.ChangeType(tokens[i], typeof(T));
			}

			return result;
		}
	}
}
