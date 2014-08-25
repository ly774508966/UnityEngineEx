using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	class Watch
	{
	}

	public class UnityClockProvider : IClockProvider
	{
		public float tick { get { return getTick(); } }



		public UnityClockProvider()
		{
		}

		float getTick()
		{
			return Time.time;
		}
	}
}
