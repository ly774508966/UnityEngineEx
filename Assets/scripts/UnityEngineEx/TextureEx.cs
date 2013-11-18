using UnityEngine;


namespace UnityEngineEx
{
	public static class TextureEx
	{
		public static Vector2 GetSize(this Texture2D texture)
		{
			return new Vector2(texture.width, texture.height);
		}

		public static Rect GetRect(this Texture texture)
		{
			return new Rect(0, 0, texture.width, texture.height);
		}
	}
}
