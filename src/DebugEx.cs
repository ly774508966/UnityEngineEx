using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	public class UnityLogger : ILogger
	{
		public void Info(string message, params object[] args)
		{
			Debug.Log(string.Format(message, args));
		}

		public void Warning(string message, params object[] args)
		{
			Debug.LogWarning(string.Format(message, args));
		}

		public void Error(string message, params object[] args)
		{
			Debug.LogError(string.Format(message, args));
		}
	}
}