using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngineEx
{
	public class BehaviourFunctionAttribute : Attribute
	{
		public string name;

		public BehaviourFunctionAttribute(string name)
		{
			this.name = name;
		}
	}
}
