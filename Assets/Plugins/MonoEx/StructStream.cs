using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SystemEx
{
	public class StructStream : IDisposable
	{
		byte[] data_;

		GCHandle handle;
		IntPtr dataPtr;


		public StructStream(byte[] data)
		{
			data_ = data;

			handle = GCHandle.Alloc(data_, GCHandleType.Pinned);
			dataPtr = handle.AddrOfPinnedObject();
		}

		public void Dispose()
		{
			handle.Free();
		}
		

		public T Read<T>() where T : struct
		{
			T r = (T) Marshal.PtrToStructure(dataPtr, typeof(T));
			Skip<T>();
			return r;
		}

		public T[] Read<T>(int count) where T : struct
		{
			T[] r = new T[count];
			for (int i = 0; i < r.Length; i++)
				r[i] = Read<T>();
			return r;
		}

		public void Skip<T>()
		{
			Skip<T>(1);
		}

		public void Skip<T>(int count)
		{
			dataPtr = new IntPtr(dataPtr.ToInt64() + count * Marshal.SizeOf(typeof(T)));
		}
	}
}
