using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
	}
}
