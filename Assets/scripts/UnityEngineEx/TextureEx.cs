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

		public static Texture2D MakeOutline(this Texture2D texture, int width)
		{
			Color[] c = texture.GetPixels();
			Color[] nc = new Color[c.Length];

			for (int y = 0; y < texture.height; y++)
				for (int x = 0; x < texture.width; x++) {
					Color cc = c[x + y * texture.width];
					if (cc.a < 0.1) {
						for (int cy = Mathf.Max(y - width, 0); cy < Mathf.Min(y + (width + 1), texture.height - 1); cy++)
							for (int cx = Mathf.Max(x - width, 0); cx < Mathf.Min(x + (width + 1), texture.width - 1); cx++)
								if (cc.a > 0.1) {
									nc[x + y * texture.width] = Color.black;
									goto skip;
								}
					}
					if (cc.a > 0) {
						cc = cc.Alpha(1.0f);
					}
					nc[x + y * texture.width] = cc;
				skip:;
				}

			texture.SetPixels(nc);
			texture.Apply();
			return texture;
		}
	}
}
