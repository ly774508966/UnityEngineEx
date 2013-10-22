using UnityEngine;
using System;

namespace UnityEngineEx
{
	public static class MeshEx
	{
		const float Pi = 3.141592f;

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
				float a = i * 2 * Pi / Sectors;
				float x = Mathf.Cos(a);
				float y = Mathf.Sin(a);
				for (int j = 0; j < Rows + 1; j++) {
					vs[vi++] = new Vector3(x*Radius, j*dH, y*Radius);
					ns[ni++] = new Vector3(x, 0, y);
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
	}
}

