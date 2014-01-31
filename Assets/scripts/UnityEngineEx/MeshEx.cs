using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngineEx
{
	public static class MeshEx
	{
		/// <summary>
		/// Translate all vertices of a Mesh.
		/// </summary>
		/// <param name="mesh"></param>
		/// <param name="Translation"></param>
		/// <returns></returns>
		public static Mesh Translate(this Mesh mesh, Vector3 Translation)
		{
			Vector3[] vertices = new Vector3[mesh.vertexCount];
			for (int i = 0; i < mesh.vertexCount; i++) {
				vertices[i] = mesh.vertices[i] + Translation;
			}

			mesh.vertices = vertices;

			return mesh;
		}

		/// <summary>
		/// Rotates all the vertices and normals of a mesh.
		/// </summary>
		/// <param name="mesh"></param>
		/// <param name="Rotation"></param>
		/// <returns></returns>
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
			Vector3[] normals = new Vector3[mesh.vertexCount];
			for (int i = 0; i < mesh.vertices.Length; i++) {
				float a = mesh.vertices[i].z * dA;
				vertices[i] = mesh.vertices[i].Rotate(new Vector3(0, 0, a));
				normals[i] = mesh.normals[i].Rotate(new Vector3(0, 0, a));
			}

			mesh.vertices = vertices;
			mesh.normals = normals;

			return mesh;
		}

		public static Mesh Twist(this Mesh mesh, Vector3 x, float dA)
		{
			Vector3[] vertices = new Vector3[mesh.vertexCount];
			Vector3[] normals = new Vector3[mesh.vertexCount];
			for (int i = 0; i < mesh.vertices.Length; i++) {
				float a = Vector3.Project(mesh.vertices[i], x).z * dA;
				vertices[i] = mesh.vertices[i].Rotate(x * a);
				normals[i] = mesh.normals[i].Rotate(x * a);
			}

			mesh.vertices = vertices;
			mesh.normals = normals;

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

		#region UV Mapping

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

		public static Mesh ProjectionUVMap(this Mesh mesh)
		{
			Vector2[] uvs = new Vector2[mesh.vertexCount];

			Rect bound = RectEx.Empty;
			for (int i = 0; i < uvs.Length; i++) {
				uvs[i] = mesh.vertices[i].xy();
				bound = bound.Extend(uvs[i]);
			}

			for (int i = 0; i < uvs.Length; i++) {
				uvs[i] = bound.Normalize(uvs[i]);
			}

			mesh.uv = uvs;

			return mesh;
		}

		public static Mesh CylindricalUVMap(this Mesh mesh)
		{
			Vector2[] uvs = new Vector2[mesh.vertexCount];

			Rect bound = RectEx.Empty;
			for (int i = 0; i < uvs.Length; i++) {
				CylinderVector3 cv = mesh.vertices[i];
				uvs[i] = new Vector2(cv.e, cv.n);
				bound = bound.Extend(uvs[i]);
				Debug.Log(uvs[i].ToString());
			}
			bound = bound.Extend(new Vector2(-Mathf.PI, bound.yMin));
			bound = bound.Extend(new Vector2(+Mathf.PI, bound.yMin));

			Debug.Log(bound);
			for (int i = 0; i < uvs.Length; i++) {
				uvs[i] = bound.Normalize(uvs[i]);
				Debug.Log(uvs[i].ToString());
			}

			mesh.uv = uvs;

			return mesh;
		}


		#endregion

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

		#region Primitive
		
		public class MeshBuilder
		{
			readonly IList<Vector3> vertices;
			readonly IList<Vector3> normals;
			readonly IList<Vector3> uvs;
			
			public MeshBuilder(int VertexCount)
			{
				vertices = new List<Vector3>(VertexCount);
				normals = new List<Vector3>(VertexCount);
			}
			
			IList<int> triangles;
			IList<int> Triangles(int TriangleCount)
			{ if (triangles == null) triangles = new List<int>();
				return triangles;
			}
		}
		
		public class MeshBuilderIterator
		{
			readonly MeshBuilder mesh;
			public MeshBuilderIterator(MeshBuilder mesh)
			{
				this.mesh = mesh;
			}
			
			public int AddVertex(Vector3 v)
			{
				return 0;
			}
			
			public int AddNormal(Vector3 v)
			{
				return 0;
			}
			
			public int AddUV(Vector2 uv)
			{
				return 0;
			}
		}
		
		public static MeshBuilderIterator Triangle(this MeshBuilderIterator mesh)
		{
			mesh.AddVertex(Vector3.up);
			mesh.AddVertex(Vector3.right.Rotate(new Vector3(0, 0, 30)));
			
			return mesh;
		}
		
		public static MeshBuilder Quad(this MeshBuilder mesh)
		{
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
					ns[ni++] = Vector3.forward;
					uvs[uvi++] = new Vector2(i / Grid.x, j / Grid.y);
				}
			}

			vi = 0;
			int ti = 0;
			int[] triangles = new int[Triangles * 3];
			for (int i = 0; i < Triangles / 2; i++, vi++) {
				if (((vi + 1) % Rows) == 0)
					vi++;

				triangles[ti++] = vi + 1;
				triangles[ti++] = vi + 0 + Rows;
				triangles[ti++] = vi + 0;
				triangles[ti++] = vi + 1;
				triangles[ti++] = vi + 1 + Rows;
				triangles[ti++] = vi + 0 + Rows;
			}

			mesh.vertices = vs;
			mesh.normals = ns;
			mesh.uv = uvs;
			mesh.triangles = triangles;

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
			int Vertices = Columns * Rows + Rows;
			int Triangles = (int)(Grid.x * Grid.y * 2);

			int vi = 0;
			Vector3[] vs = new Vector3[Vertices];
			int ni = 0;
			Vector3[] ns = new Vector3[Vertices];
			int uvi = 0;
			Vector2[] uvs = new Vector2[Vertices];


			float a = 0;
			float dA = 2 * Mathf.PI / Columns;
			float dH = Dimensions.y / Grid.y;
			for (int i = 0; i < Columns; i++, a += dA) {
				float x = Mathf.Cos(a);
				float y = Mathf.Sin(a);
				for (int j = 0; j < Rows; j++) {
					vs[vi++] = new Vector3(x*Dimensions.x, y*Dimensions.x, j*dH);
					ns[ni++] = new Vector3(x, y, 0);
					uvs[uvi++] = new Vector2(i / (float)(Columns), j / Grid.y);
				}
			}
			{
				a = 0;
				float x = Mathf.Cos(a);
				float y = Mathf.Sin(a);
				for (int j = 0; j < Rows; j++) {
					vs[vi++] = new Vector3(x * Dimensions.x, y * Dimensions.x, j * dH);
					ns[ni++] = new Vector3(x, y, 0);
					uvs[uvi++] = new Vector2(1.0f, j / Grid.y);
				}
			}


			vi = 0;
			int ti = 0;
			int[] triangles = new int[Triangles * 3];
			for (int i = 0; i < Triangles / 2; i++, vi++) {
				if (((vi + 1) % Rows) == 0)
					vi++;

				triangles[ti++] = vi + 0;
				triangles[ti++] = vi + 0 + Rows;
				triangles[ti++] = vi + 1;
				triangles[ti++] = vi + 1;
				triangles[ti++] = vi + 0 + Rows;
				triangles[ti++] = vi + 1 + Rows;
			}

			mesh.vertices = vs;
			mesh.normals = ns;
			mesh.uv = uvs;
			mesh.triangles = triangles;

			return mesh;
		}

		#region Platonic solids

		public static Mesh Tetrahedron(this Mesh mesh)
		{
			return mesh;
		}

		public static Mesh Icosahedron(this Mesh mesh)
		{
			float g = (1 + Mathf.Sqrt(5)) / 2;

			Vector3[] vs = {
				new Vector3( 0, +1, -g),
				new Vector3( 0, -1, -g),
				new Vector3(+g,  0, -1),
				new Vector3(-g,  0, -1),
				new Vector3(-1, -g,  0),
				new Vector3(+1, -g,  0),
				new Vector3(-1, +g,  0),
				new Vector3(+1, +g,  0),
				new Vector3(+g,  0, +1),
				new Vector3(-g,  0, +1),
				new Vector3( 0, +1, +g),
				new Vector3( 0, -1, +g),
			};

			Vector3[] ns = new Vector3[vs.Length];
			for (int i = 0; i < ns.Length; i++) {
				ns[i] = vs[i].normalized;
			}



			int[] triangles = {
				0, 2, 1,
				0, 1, 3,
				0, 7, 2,
				0, 3, 6,
				0, 6, 7,
				1, 2, 5,
				1, 4, 3,
				1, 5, 4,
				9, 6, 3,
				9, 3, 4,
				8, 2, 7,
				8, 5, 2,
				10, 7, 6,
				10, 6, 9,
				10, 8, 7,
				10, 9, 11,
				10, 11, 8,
				11, 9, 4,
				11, 4, 5,
				11, 5, 8,
			};

			mesh.vertices = vs;
			mesh.normals = ns;
			mesh.triangles = triangles;

			return mesh;
		}

		#endregion

		#endregion

		public static Mesh Box(this Mesh mesh, Vector3 Dimensions, Vector3 Grid)
		{
			mesh.Recangle(Dimensions.xy(), Grid.xy()).Translate(Vector3.forward * Dimensions.z / 2).ShiftUV(new Rect(0.5f, 0, 0.5f, 0.25f)).Add(
				new Mesh().Recangle(Dimensions.xz(), Grid.xz()).Rotate(Quaternion.Euler(new Vector3(90, 0, 0))).Translate(-Vector3.up * Dimensions.y / 2).ShiftUV(new Rect(0, 0.25f, 0.5f, 0.25f))).Add(
				new Mesh().Recangle(Dimensions.zy(), Grid.zy()).Rotate(Quaternion.Euler(new Vector3(0, 270, 0))).Translate(Vector3.right * Dimensions.x / 2).ShiftUV(new Rect(0, 0.5f, 0.5f, 0.25f))).Add(
				new Mesh().Recangle(Dimensions.xy(), Grid.xy()).Rotate(Quaternion.Euler(new Vector3(180, 0, 0))).Translate(-Vector3.forward * Dimensions.z / 2).ShiftUV(new Rect(0, 0, 0.5f, 0.25f))).Add(
				new Mesh().Recangle(Dimensions.xz(), Grid.xz()).Rotate(Quaternion.Euler(new Vector3(270, 0, 0))).Translate(Vector3.up * Dimensions.y / 2).ShiftUV(new Rect(0.5f, 0.25f, 0.5f, 0.25f))).Add(
				new Mesh().Recangle(Dimensions.zy(), Grid.zy()).Rotate(Quaternion.Euler(new Vector3(0, 90, 0))).Translate(Vector3.right * Dimensions.x / 2).ShiftUV(new Rect(0.5f, 0.5f, 0.5f, 0.25f)));

			return mesh;
		}
	}
}

