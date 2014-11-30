using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngineEx
{
	public interface IConstructable
	{
		void Constructor(params object[] args);
	}
}
