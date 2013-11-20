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

		public static Texture2D Trim(this Texture2D texture)
		{
			Color[] c = texture.GetPixels();
			int minx = texture.width;
			int miny = texture.height;
			int maxx = 0;
			int maxy = 0;

			for (int y = 0; y < texture.height; y++)
				for (int x = 0; x < texture.width; x++) {
					Color cc = c[x + y * texture.width];
					if (cc.a > 0) {
						if (minx > x) minx = Mathf.Min(x - 1, minx);
						if (miny > y) miny = Mathf.Min(y - 1, miny);
						if (maxx < x) maxx = Mathf.Max(x + 1, maxx);
						if (maxy < y) maxy = Mathf.Max(y + 1, maxy);
					}
				}

			maxx++; maxy++;

			if (minx < 0) minx = 0;
			if (miny < 0) miny = 0;
			if (maxx > texture.width) maxx = texture.width;
			if (maxy > texture.height) maxx = texture.height;

			int dx = maxx - minx;
			int dy = maxy - miny;
			Color[] nc = new Color[dx * dy];
			for (int y = 0; y < dy; y++)
				for (int x = 0; x < dx; x++) {
					nc[x + y * dx] = c[minx + x + (miny + y) * texture.width];
				}

			Texture2D newtexture = new Texture2D(dx, dy, texture.format, false);
			newtexture.filterMode = texture.filterMode;
			newtexture.wrapMode = texture.wrapMode;
			newtexture.SetPixels(nc);
			newtexture.Apply();
			return newtexture;
		}

		public static Texture2D MakeOutline(this Texture2D texture, int width)
		{
			Color[] c = texture.GetPixels();
			Color[] nc = new Color[c.Length];

			for (int y = 0; y < texture.height; y++)
				for (int x = 0; x < texture.width; x++) {
					Color cc = c[x + y * texture.width];
					if (cc.a < 0.1) {
						for (int cy = Mathf.Max(y - width, 0); cy <= Mathf.Min(y + width, texture.height - 1); cy++)
							for (int cx = Mathf.Max(x - width, 0); cx <= Mathf.Min(x + width, texture.width - 1); cx++) {
								cc = c[cx + cy * texture.width];
								if (cc.a > 0.1) {
									nc[x + y * texture.width] = Color.black;
									goto skip;
								}
							}
					}
					if (cc.a > 0) {
						cc = cc.Alpha(1.0f);
					}
					nc[x + y * texture.width] = cc;
				skip:;
				}

			texture.SetPixels(nc, 0);
			texture.Apply();
			return texture;
		}
	}
}
