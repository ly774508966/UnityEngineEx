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

		public static Mesh Finalize(this Mesh mesh)
		{
			mesh.Optimize();
			mesh.RecalculateBounds();
			return mesh;
		}

		public static Mesh Cylinder(this Mesh mesh, float Radius, int Sectors, int Rows)
		{
			return mesh.Cylinder(Radius, Sectors, Rows, 1.0f);
		}

		public static Mesh Cylinder(this Mesh mesh, float Radius, int Sectors, int Rows, float Height)
		{
			float dH = Height / Rows;
			int Vertices = Sectors * (Rows + 1);
			int Triangles = Sectors * Rows * 2;
			int vi = 0;
			Vector3[] vs = new Vector3[Vertices];
			int ni = 0;
			Vector3[] ns = new Vector3[Vertices];
			int uvi = 0;
			Vector2[] uvs = new Vector2[Vertices];
			int ti = 0;
			int[] triangles = new int[Triangles * 3];
			for (int i = 0; i < Sectors; i++) {
				float a = i * 2 * Mathf.PI / Sectors;
				float x = Mathf.Cos(a);
				float y = Mathf.Sin(a);
				for (int j = 0; j < Rows + 1; j++) {
					vs[vi++] = new Vector3(x*Radius, y*Radius, j*dH);
					ns[ni++] = new Vector3(x, y, 0);
					uvs[uvi++] = new Vector2(i % 2, j % 2);
				}
				if (i > 0) {
					for (int d = (Rows + 1) * 2; d > (Rows + 1) * 2 - Rows; d--) {
						triangles[ti++] = (vi - d) + 0;
						triangles[ti++] = (vi - d) + 1;
						triangles[ti++] = (vi - d) + 0 + (Rows + 1);
						triangles[ti++] = (vi - d) + 0 + (Rows + 1);
						triangles[ti++] = (vi - d) + 1;
						triangles[ti++] = (vi - d) + 1 + (Rows + 1);
					}
				}
			}
			for (int d = (Rows + 1); d > 1; d--) {
				triangles[ti++] = (vi - d) + 0;
				triangles[ti++] = (vi - d) + 1;
				triangles[ti++] = ( 0 - d) + 0 + (Rows + 1);
				triangles[ti++] = ( 0 - d) + 0 + (Rows + 1);
				triangles[ti++] = (vi - d) + 1;
				triangles[ti++] = ( 0 - d) + 1 + (Rows + 1);
			}

			mesh.vertices = vs;
			mesh.normals = ns;
			mesh.uv = uvs;
			mesh.triangles = triangles;

			return mesh;
		}

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
	}
}

