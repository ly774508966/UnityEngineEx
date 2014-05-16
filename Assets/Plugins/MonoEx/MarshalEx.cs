using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SystemEx
{
	class MarshalEx
	{
		public static int SizeOf<T>() { return Marshal.SizeOf(typeof(T)); }
	}
}
