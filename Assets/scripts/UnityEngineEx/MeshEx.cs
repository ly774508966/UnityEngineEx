using UnityEngine;
using System;

namespace UnityEngineEx
{
	public static class MeshEx
	{
		public static Mesh Translate(this Mesh mesh, Vector3 Translation)
		{
			Vector3[] vertices = new Vector3[mesh.vertexCount];
			for (int i = 0; i < mesh.vertexCount; i++) {
				vertices[i] = mesh.vertices[i] + Translation;
			}

			mesh.vertices = vertices;

			return mesh;
		}

		public static Mesh Rotate(this Mesh mesh, Quaternion Rotation)
		{
			Vector3[] vertices = new Vector3[mesh.vertexCount];
			for (int i = 0; i < mesh.vertexCount; i++) {
				vertices[i] = Rotation * mesh.vertices[i];
			}

			Vector3[] normals = new Vector3[mesh.normals.Length];
			for (int i = 0; i < mesh.normals.Length; i++) {
				normals[i] = Rotation * mesh.normals[i];
			}

			mesh.vertices = vertices;
			mesh.normals = normals;

			return mesh;
		}

		public static Mesh Twist(this Mesh mesh, float dA)
		{
			Vector3[] vertices = new Vector3[mesh.vertexCount];
			for (int i = 0; i < mesh.vertices.Length; i++) {
				float a = mesh.vertices[i].z * dA;
				vertices[i] = mesh.vertices[i].Rotate(new Vector3(0, 0, a));
			}

			mesh.vertices = vertices;

			return mesh;
		}

		public static Mesh Twist(this Mesh mesh, Vector3 x, float dA)
		{
			Vector3[] vertices = new Vector3[mesh.vertexCount];
			for (int i = 0; i < mesh.vertices.Length; i++) {
				float a = Vector3.Project(mesh.vertices[i], x).z * dA;
				vertices[i] = mesh.vertices[i].Rotate(x * a);
			}

			mesh.vertices = vertices;

			return mesh;
		}

		public static Mesh Color(this Mesh mesh, Color color)
		{
			Color[] colors = new Color[mesh.vertexCount];
			for (int i = 0; i < colors.Length; i++)
				colors[i] = color;
			mesh.colors = colors;
			return mesh;
		}

		public static Mesh ShiftUV(this Mesh mesh, Rect uv)
		{
			Vector2 shift = new Vector2(uv.xMin, uv.yMin);
			Vector2 scale = new Vector2(uv.width, uv.height);
			Vector2[] uvs = new Vector2[mesh.uv.Length];
			for (int i = 0; i < mesh.uv.Length; i++) {
				uvs[i] = shift + mesh.uv[i].Mul(scale);
			}

			mesh.uv = uvs;

			return mesh;
		}

		public static Mesh Add(this Mesh mesh, Mesh add)
		{
			Vector3[] vs = new Vector3[mesh.vertices.Length + add.vertices.Length];
			Array.Copy(mesh.vertices, vs, mesh.vertices.Length); Array.Copy(add.vertices, 0, vs, mesh.vertices.Length, add.vertices.Length);
			Vector3[] ns = new Vector3[mesh.normals.Length + add.normals.Length];
			Array.Copy(mesh.normals, ns, mesh.normals.Length); Array.Copy(add.normals, 0, ns, mesh.normals.Length, add.normals.Length);
			Vector2[] uvs = new Vector2[mesh.uv.Length + add.uv.Length];
			Array.Copy(mesh.uv, uvs, mesh.uv.Length); Array.Copy(add.uv, 0, uvs, mesh.uv.Length, add.uv.Length);

			int tc = mesh.vertices.Length;
			int[] ts = new int[mesh.triangles.Length + add.triangles.Length];
			Array.Copy(mesh.triangles, ts, mesh.triangles.Length); Array.Copy(add.triangles, 0, ts, mesh.triangles.Length, add.triangles.Length);
			for (int i = mesh.triangles.Length; i < ts.Length; i++)
				ts[i] += tc;

			mesh.vertices = vs;
			mesh.normals = ns;
			mesh.uv = uvs;
			mesh.triangles = ts;

			return mesh;
		}

		public static Mesh Finalize(this Mesh mesh)
		{
			mesh.Optimize();
			mesh.RecalculateBounds();
			return mesh;
		}

		public static Mesh Cylinder(this Mesh mesh, float Radius, Vector2 Grid)
		{
			return mesh.Cylinder(new Vector2(Radius, 1.0f), Grid);
		}

		/// <summary>
		/// Cylinder the specified mesh, Dimensions and Grid.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		/// <param name="Dimensions">Dimensions. X - radius. Y - Length.</param>
		/// <param name="Grid">Grid.</param>
		public static Mesh Cylinder(this Mesh mesh, Vector2 Dimensions, Vector2 Grid)
		{
			int Columns = (int)(Grid.x);
			int Rows = (int)(Grid.y + 1);
			float dH = Dimensions.y / Grid.y;
			int Vertices = Columns * Rows;
			int Triangles = (int)(Grid.x * Grid.y * 2);

			int vi = 0;
			Vector3[] vs = new Vector3[Vertices];
			int ni = 0;
			Vector3[] ns = new Vector3[Vertices];
			int uvi = 0;
			Vector2[] uvs = new Vector2[Vertices];
			for (int i = 0; i < Columns; i++) {
				float a = i * 2 * Mathf.PI / Columns;
				float x = Mathf.Cos(a);
				float y = Mathf.Sin(a);
				for (int j = 0; j < Rows; j++) {
					vs[vi++] = new Vector3(x*Dimensions.x, y*Dimensions.x, j*dH);
					ns[ni++] = new Vector3(x, y, 0);
					uvs[uvi++] = new Vector2(i % 2, j % 2);
				}
			}

			vi = 0;
			int ti = 0;
			int[] triangles = new int[Triangles * 3];
			for (int i = 0; i < Triangles / 2; i++, vi++) {
				if (((vi + 1) % Rows) == 0)
					vi++;

				triangles[ti++] = vi + 0;
				triangles[ti++] = (vi + 0 + Rows) % vs.Length;
				triangles[ti++] = vi + 1;
				triangles[ti++] = vi + 1;
				triangles[ti++] = (vi + 0 + Rows) % vs.Length;
				triangles[ti++] = (vi + 1 + Rows) % vs.Length;
			}

			mesh.vertices = vs;
			mesh.normals = ns;
			mesh.uv = uvs;
			mesh.triangles = triangles;

			return mesh;
		}

		public static Mesh Recangle(this Mesh mesh)
		{
			return mesh.Recangle(Vector2.one, Vector2.one);
		}

		public static Mesh Recangle(this Mesh mesh, float width)
		{
			return mesh.Recangle(new Vector2(width, 1.0f), Vector2.one);
		}

		public static Mesh Recangle(this Mesh mesh, Vector2 Dimensions)
		{
			return mesh.Recangle(Dimensions, Vector2.one);
		}

		/// <summary>
		/// Recangle the specified mesh, Dimensions and Grid.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		/// <param name="Dimensions">Dimensions. X - Width. Y - Height.</param>
		/// <param name="Grid">Grid.</param>
		public static Mesh Recangle(this Mesh mesh, Vector2 Dimensions, Vector2 Grid)
		{
			Vector2 dV = new Vector2(Dimensions.x / Grid.x, Dimensions.y / Grid.y);
			int Columns = (int)(Grid.x + 1);
			int Rows = (int)(Grid.y + 1);
			int Vertices = Columns * Rows;
			int Triangles = (int)(Grid.x * Grid.y * 2);
			int vi = 0;
			Vector3[] vs = new Vector3[Vertices];
			int ni = 0;
			Vector3[] ns = new Vector3[Vertices];
			int uvi = 0;
			Vector2[] uvs = new Vector2[Vertices];
			for (int i = 0; i < Columns; i++) {
				float x = i * dV.x;
				for (int j = 0; j < Rows; j++) {
					float y = j * dV.y;
					vs[vi++] = new Vector3(x, y, 0).Sub(Dimensions/2);
					ns[ni++] = new Vector3(0, 0, -1);
					uvs[uvi++] = new Vector2(i % 2, j % 2);
				}
			}

			vi = 0;
			int ti = 0;
			int[] triangles = new int[Triangles * 3];
			for (int i = 0; i < Triangles / 2; i++, vi++) {
				if (((vi + 1) % Rows) == 0)
					vi++;

				triangles[ti++] = vi + 0;
				triangles[ti++] = vi + 1;
				triangles[ti++] = vi + 0 + Rows;
				triangles[ti++] = vi + 0 + Rows;
				triangles[ti++] = vi + 1;
				triangles[ti++] = vi + 1 + Rows;
			}

			mesh.vertices = vs;
			mesh.normals = ns;
			mesh.uv = uvs;
			mesh.triangles = triangles;

			return mesh;
		}

		public static Mesh Box(this Mesh mesh, Vector3 Dimensions, Vector3 Grid)
		{
			mesh.Recangle(Dimensions.xy(), Grid.xy()).Translate(-Vector3.forward * Dimensions.z / 2).ShiftUV(new Rect(0.5f, 0, 0.5f, 0.25f)).Add(
				new Mesh().Recangle(Dimensions.xz(), Grid.xz()).Rotate(Quaternion.Euler(new Vector3(90, 0, 0))).Translate(Vector3.up * Dimensions.y / 2).ShiftUV(new Rect(0, 0.25f, 0.5f, 0.25f))).Add(
				new Mesh().Recangle(Dimensions.zy(), Grid.zy()).Rotate(Quaternion.Euler(new Vector3(0, 270, 0))).Translate(Vector3.right * Dimensions.x / 2).ShiftUV(new Rect(0, 0.5f, 0.5f, 0.25f))).Add(
				new Mesh().Recangle(Dimensions.xy(), Grid.xy()).Rotate(Quaternion.Euler(new Vector3(180, 0, 0))).Translate(Vector3.forward * Dimensions.z / 2).ShiftUV(new Rect(0, 0, 0.5f, 0.25f))).Add(
				new Mesh().Recangle(Dimensions.xz(), Grid.xz()).Rotate(Quaternion.Euler(new Vector3(270, 0, 0))).Translate(-Vector3.up * Dimensions.y / 2).ShiftUV(new Rect(0.5f, 0.25f, 0.5f, 0.25f))).Add(
				new Mesh().Recangle(Dimensions.zy(), Grid.zy()).Rotate(Quaternion.Euler(new Vector3(0, 90, 0))).Translate(-Vector3.right * Dimensions.x / 2).ShiftUV(new Rect(0.5f, 0.5f, 0.5f, 0.25f)));

			return mesh;
		}
	}
}

