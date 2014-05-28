using SystemEx;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEngineEx
{
    public static class DebugEx
    {
        public static void Log(string message, params object[] args)
        {
            Debug.Log(string.Format(message, args));
        }
    }

    public class Logger : ILogger
    {
        public void Log(string message, params object[] args)
        {
            Debug.Log(string.Format(message, args));
        }
    }
}
