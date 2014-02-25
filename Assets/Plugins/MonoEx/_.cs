using System;

namespace SystemEx
{
	public static class _
	{
		public static Action<T> a<T>(Action<T> lambda)
		{
			return lambda;
		}
	}
}
