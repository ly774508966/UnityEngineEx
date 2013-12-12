using UnityEngine;

namespace UnityEngineEx
{
	public static class AnimationEx
	{
		/// <summary>
		/// Get AnimationState of Animation by index.
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static AnimationState GetState(this Animation animation, int index)
		{
			int i = 0;
			foreach (AnimationState state in animation) {
				if (i == index)
					return state;
				i++;
			}

			return null;
		}
	}
}
