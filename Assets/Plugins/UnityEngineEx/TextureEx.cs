using MathEx;
using UnityEngine;

namespace UnityEngineEx
{
	public static class TextureEx
	{
		/// <summary>
		/// Returns texture size in Vector2 struct.
		/// </summary>
		/// <param name="texture"></param>
		/// <returns></returns>
		public static Vector2 GetSize(this Texture2D texture)
		{
			return new Vector2(texture.width, texture.height);
		}

		/// <summary>
		/// Returns texture size in Rect struct.
		/// </summary>
		/// <param name="texture"></param>
		/// <returns></returns>
		public static Rect GetRect(this Texture texture)
		{
			return new Rect(0, 0, texture.width, texture.height);
		}

		public static Texture2D Fill(this Texture2D texture, Color color)
		{
			Color[] pixels = texture.GetPixels();

			for (int i = 0; i < pixels.Length; i++)
				pixels[i] = color;

			texture.SetPixels(pixels);
			texture.Apply();

			return texture;
		}

		public static Texture2D SetColumn(this Texture2D texture, int x, Color color)
		{
			for (int i = 0; i < texture.height; i++)
				texture.SetPixel(x, i, color);
			return texture;
		}

		public static Texture2D SetLine(this Texture2D texture, int y, Color color)
		{
			for (int i = 0; i < texture.width; i++)
				texture.SetPixel(i, y, color);
			return texture;
		}

		public static Texture2D Circle(this Texture2D texture, Vector2 position, float radius, Color color)
		{
			return texture;
		}

		public static Texture2D Ellipse(this Texture2D texture, Vector2 position, Vector2 radius, Color color)
		{
			var p = position;
			var s = texture.GetSize();
			int w = (int) s.x;
			var sx = new Vector2(0, s.x - 1);
			var sy = new Vector2(0, s.y - 1);
			Color[] pixels = texture.GetPixels();

			// from: ftp://pc.fk0.name/pub/books/programming/bezier-ellipse.pdf

			int cX = MathfEx.Round(position.x);
			int cY = MathfEx.Round(position.y);
			int TwoASquare = MathfEx.Round(2*radius.x*radius.x);
			int TwoBSquare = MathfEx.Round(2*radius.y*radius.y);
			int X = MathfEx.Round(radius.x);
			int Y = 0;
			int XChange = MathfEx.Round(radius.y*radius.y*(1 - 2*radius.x));
			int YChange = MathfEx.Round(radius.x*radius.x);
			int EllipseError = 0;
			int StoppingX = MathfEx.Round(TwoBSquare*radius.x);
			int StoppingY = 0;
			
			int o;
			while (StoppingX >= StoppingY) {
				o = sy.Clamp(cY - Y) * w;
				for (int i = sx.Clamp(cX - X); i <= sx.Clamp(cX + X); i++)
					pixels[i + o] = color;
				o = sy.Clamp(cY + Y) * w;
				for (int i = sx.Clamp(cX - X); i <= sx.Clamp(cX + X); i++)
					pixels[i + o] = color;

				Y++;
				StoppingY += TwoASquare;
				EllipseError += YChange;
				YChange += TwoASquare;
				if ((2*EllipseError + XChange) > 0 ) {
					X--;
					StoppingX -= TwoBSquare;
					EllipseError += XChange;
					XChange += TwoBSquare;
				}
			}

			X = 0;
			Y = MathfEx.Round(radius.y);
			XChange = MathfEx.Round(radius.y*radius.y);
			YChange = MathfEx.Round(radius.x*radius.x*(1 - 2*radius.y));
			EllipseError = 0;
			StoppingX = 0;
			StoppingY = MathfEx.Round(TwoASquare*radius.y);
			while ( StoppingX <= StoppingY ) {
				o = sy.Clamp(cY - Y) * w;
				for (int i = sx.Clamp(cX - X); i <= sx.Clamp(cX + X); i++)
					pixels[i + o] = color;
				o = sy.Clamp(cY + Y) * w;
				for (int i = sx.Clamp(cX - X); i <= sx.Clamp(cX + X); i++)
					pixels[i + o] = color;

				X++;
				StoppingX += TwoBSquare;
				EllipseError += XChange;
				XChange += TwoBSquare;
				if ((2*EllipseError + YChange) > 0 ) {
					Y--;
					StoppingY -= TwoASquare;
					EllipseError += YChange;
					YChange += TwoASquare;
				}
			}

			texture.SetPixels(pixels);
			texture.Apply();

			return texture;
		}

		/// <summary>
		/// Cuts off empty bixel border from a texture.
		/// </summary>
		/// <param name="texture"></param>
		/// <returns></returns>
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

			if (minx > maxx || miny > maxy)
				return texture;

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
						float a = 0;
						for (int cy = Mathf.Max(y - width, 0); cy <= Mathf.Min(y + width, texture.height - 1); cy++)
							for (int cx = Mathf.Max(x - width, 0); cx <= Mathf.Min(x + width, texture.width - 1); cx++)
								a = Mathf.Max(c[cx + cy * texture.width].a, a);

						if (a > 0.1) {
							nc[x + y * texture.width] = Color.black.Alpha(a);
							goto skip;
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
