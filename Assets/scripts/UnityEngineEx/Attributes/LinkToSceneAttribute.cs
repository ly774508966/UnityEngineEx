using System;

namespace UnityEngineEx
{
	public class LinkToSceneAttribute : Attribute
	{
		public string name;
		
		public LinkToSceneAttribute(string name)
		{
			this.name = name;
		}
	}
}