using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemEx
{
	public class LambdaComparer<T> : IComparer<T>
	{
		private Comparison<T> c;

		public LambdaComparer(Comparison<T> comparison)
		{
			c = comparison;
		}

		public int Compare(T x, T y)
		{
			return c(x, y);
		}
	}

	public static class LambdaComparer
	{
		static public LambdaComparer<T> Create<T>(Comparison<T> comparison)
		{
			return new LambdaComparer<T>(comparison);
		}
	}
}
