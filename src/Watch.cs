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
		public float hwtick { get { return Time.realtimeSinceStartup; } }


		public UnityClockProvider()
		{
		}

		float getTick()
		{
			return Time.time;
		}
	}
}
