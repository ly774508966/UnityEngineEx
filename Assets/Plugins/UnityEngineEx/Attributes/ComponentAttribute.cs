using System;

namespace UnityEngineEx
{
	public class ComponentAttribute : Attribute
	{
		public string name = null;
		
		public ComponentAttribute()
		{
		}

		public ComponentAttribute(string name)
		{
			this.name = name;
		}
	}

	public class AddComponentAttribute : Attribute
	{
		public AddComponentAttribute()
		{
		}
	}
}