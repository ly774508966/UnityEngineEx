using System;

namespace UnityEngineEx
{
	public class LinkToSceneAttribute : Attribute
	{
		public string name = null;
		
		public LinkToSceneAttribute()
		{
		}

		public LinkToSceneAttribute(string name)
		{
			this.name = name;
		}
	}
}