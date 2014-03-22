using System;

namespace SystemEx
{
	public static class _
	{
		/*
		public static Action<T> a<T>(Action<T> lambda) { return lambda; }
		public static Action<T1, T2> a<T1, T2>(Action<T1, T2> lambda) { return lambda; }
		public static Action<T1, T2, T3> a<T1, T2, T3>(Action<T1, T2, T3> lambda) { return lambda; }
		public static Action<T1, T2, T3, T4> a<T1, T2, T3, T4>(Action<T1, T2, T3, T4> lambda) { return lambda; }
		public static Action<T1, T2, T3, T4, T5> a<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> lambda) { return lambda; }
		public static Action<T1, T2, T3, T4, T5, T6> a<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> lambda) { return lambda; }
		public static Action<T1, T2, T3, T4, T5, T6, T7> a<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> lambda) { return lambda; }
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8> a<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> lambda) { return lambda; }
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> a<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> lambda) { return lambda; }
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> a<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> lambda) { return lambda; }
		*/

		public static ActionContainer a<T>(Action<T> lambda) { return new ActionContainer((Delegate)lambda, typeof(T)); }
		public static ActionContainer a<T1, T2>(Action<T1, T2> lambda) { return new ActionContainer((Delegate)lambda, typeof(T1), typeof(T2)); }
		public static ActionContainer a<T1, T2, T3>(Action<T1, T2, T3> lambda) { return new ActionContainer((Delegate)lambda, typeof(T1), typeof(T2), typeof(T3)); }
		public static ActionContainer a<T1, T2, T3, T4>(Action<T1, T2, T3, T4> lambda) { return new ActionContainer((Delegate)lambda, typeof(T1), typeof(T2), typeof(T3), typeof(T4)); }
		public static ActionContainer a<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> lambda) { return new ActionContainer((Delegate)lambda, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)); }
		public static ActionContainer a<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> lambda) { return new ActionContainer((Delegate)lambda, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)); }
		public static ActionContainer a<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> lambda) { return new ActionContainer((Delegate)lambda, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)); }
		public static ActionContainer a<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> lambda) { return new ActionContainer((Delegate)lambda, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)); }
		public static ActionContainer a<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> lambda) { return new ActionContainer((Delegate)lambda, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9)); }
		public static ActionContainer a<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> lambda) { return new ActionContainer((Delegate)lambda, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10)); }
	}
}
