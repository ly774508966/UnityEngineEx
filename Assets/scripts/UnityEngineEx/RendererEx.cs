using UnityEngine;

namespace UnityEngineEx
{
	public static class RendererEx
	{
		public static Renderer SetSoringLayer(this Renderer renderer, string name, int order)
		{
			renderer.sortingLayerName = name;
			renderer.sortingOrder = order;
			return renderer;
		}
	}
}

