using System;
using System.Collections.Generic;
using SystemEx;
using UnityEngine;
using UnityEngineEx;

namespace ProcGenEx
{
	public class MeshBuilder
	{
		public List<Vector3> vertices = null;
		public List<Vector3> normals = null;
		public List<Vector2> uvs = null;
		public List<int> triangles = null;

		private List<List<int>> v2t = null;
			
		public MeshBuilder(int VertexCount, int TriangleCount)
		{
			vertices = new List<Vector3>(VertexCount);
			normals = new List<Vector3>(VertexCount);
			uvs = new List<Vector2>(VertexCount);
			triangles = new List<int>(TriangleCount * 3);

			v2t = new List<List<int>>(VertexCount);
		}

		public Mesh ToMesh()
		{
			Mesh m = new Mesh();

			m.vertices = vertices.ToArray();
			m.normals = normals.ToArray();
			m.uv = uvs.ToArray();
			m.triangles = triangles.ToArray();

			m.Finalize();
			
			return m;
		}

		public void Grow(int VertexCount, int TriangleCount)
		{
			int dvc = Math.Min(vertices.Capacity, vertices.Count + VertexCount);
			int dtc = Math.Min(triangles.Capacity, triangles.Count + TriangleCount);
			vertices.Capacity = dvc;
			normals.Capacity = dvc;
			uvs.Capacity = dvc;
			triangles.Capacity = dtc;
		}

#region Simple figures

		public static MeshBuilder Triangle(Vector3 a, Vector3 b, Vector3 c)
		{
			MeshBuilder mb = new MeshBuilder(3, 1);

			var n = Vector3.Cross((c - a), (b - a));

			mb.CreateVertex(a, n);
			mb.CreateVertex(b, n);
			mb.CreateVertex(c, n);

			mb.MakeTriangle(0, 1, 2);

			return mb;
		}

		public static MeshBuilder Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			MeshBuilder mb = new MeshBuilder(4, 2);

			var n = Vector3.Cross((c - a), (b - a));

			mb.CreateVertex(a, n);
			mb.CreateVertex(b, n);
			mb.CreateVertex(c, n);
			mb.CreateVertex(d, n);

			mb.MakeQuad(0, 1, 2, 3);

			return mb;
		}

#endregion

		public int CreateVertex(Vector3 v, Vector3 n)
		{
			return CreateVertex(v, n, Vector2.zero);
		}

		public int CreateVertex(Vector3 v, Vector3 n, Vector2 u)
		{
			var vi = vertices.Count;

			vertices.Add(v);
			normals.Add(n);
			uvs.Add(u);
			v2t.Add(new List<int>());

			return vi;
		}

		public int CopyVertex(int vi, Vector3 dv)
		{
			return CreateVertex(vertices[vi] + dv, normals[vi], uvs[vi]);
		}

		public void MakeTriangle(int a, int b, int c)
		{
			var ti = triangles.Count;			

			triangles.Add(a);
			triangles.Add(b);
			triangles.Add(c);

			v2t[a].Add(ti);
			v2t[b].Add(ti);
			v2t[c].Add(ti);
		}

		public void MakeQuad(int a, int b, int c, int d)
		{
			MakeTriangle(a, b, c);
			MakeTriangle(a, c, d);
		}

		public void MakeFan(params int[] ps)
		{
			for (int i = 1; i < ps.Length - 1; i++) {
				MakeTriangle(ps[0], ps[i], ps[i + 1]);
			}
		}

		public void Extrude(int[] contour, Vector3 direction, int steps)
		{
			// light overgrow in triangle count expected.
			Grow(contour.Length * steps, contour.Length * steps * 2);

			Vector3 dv = direction / steps;
			for (int si = 0; si < steps; si++) {
				int pv = CopyVertex(contour[0], dv);
				for (int i = 1; i < contour.Length; i++) {
					int cv = CopyVertex(contour[i], dv);

					MakeQuad(contour[i-1], pv, cv, contour[i]);

					contour[i - 1] = pv;
					pv = cv;
				}
				contour[contour.Length - 1] = pv;
			}
		}

		public void Slice(Plane plane)
		{
			int[] v2v = new int[vertices.Count].Initialize(-1);
			List<Vector3> vs = vertices;
			List<Vector3> ns = normals;
			List<Vector2> us = uvs;
			List<int> ts = triangles;

			vertices = new List<Vector3>(vs.Count);;
			normals = new List<Vector3>(vs.Count);
			uvs = new List<Vector2>(vs.Count);
			triangles = new List<int>(ts.Count);


			for (int i = 0; i < vs.Count; i++) {
				if (plane.GetDistanceToPoint(vs[i]) >= 0) {
					v2v[i] = CreateVertex(vs[i], ns[i], us[i]);					
				}
			}


			for (int i = 0; i < ts.Count; i += 3) {
				int st = ((v2v[ts[i]] < 0 ? 0 : 1) << 0) + ((v2v[ts[i + 1]] < 0 ? 0 : 1) << 1) + ((v2v[ts[i + 2]] < 0 ? 0 : 1) << 2);
				
				if (st == 0)
					continue;
				if (st == 7) {
					MakeTriangle(v2v[ts[i]], v2v[ts[i + 1]], v2v[ts[i + 2]]);
					continue;
				}

				Vector3 ab, bc, ca;
				float abd, bcd, cad;
				bool abi = plane.Intersect(vs[ts[i]], vs[ts[i + 1]], out ab, out abd);
				bool bci = plane.Intersect(vs[ts[i + 1]], vs[ts[i + 2]], out bc, out bcd);
				bool cai = plane.Intersect(vs[ts[i + 2]], vs[ts[i]], out ca, out cad);


				List<int> nvs = new List<int>(4);

				if (!(v2v[ts[i]] < 0)) {
					nvs.Add(v2v[ts[i]]);
				}
				if (abi && (abd > 0 && abd < 1)) {
					nvs.Add(CreateVertex(ab, Vector3.Slerp(ns[ts[i]], ns[ts[i + 1]], abd)));
				}

				if (!(v2v[ts[i + 1]] < 0)) {
					nvs.Add(v2v[ts[i + 1]]);
				}
				if (bci && (bcd > 0 && bcd < 1)) {
					nvs.Add(CreateVertex(bc, Vector3.Slerp(ns[ts[i + 1]], ns[ts[i + 2]], bcd)));
				}

				if (!(v2v[ts[i + 2]] < 0)) {
					nvs.Add(v2v[ts[i + 2]]);
				}
				if (cai && (cad > 0 && cad < 1)) {
					nvs.Add(CreateVertex(ca, Vector3.Slerp(ns[ts[i + 2]], ns[ts[i]], cad)));
				}

				MakeFan(nvs.ToArray());
			}
		}

		public int[] Select(Plane plane)
		{
			List<int> result = new List<int>(vertices.Count);

			for (int i = 0; i < vertices.Count; i++) {
				if (plane.GetDistanceToPoint(vertices[i]) >= 0) {
					result.Add(i);
				}
			}

			return result.ToArray();
		}

		public int[] Project(Plane plane)
		{
			List<int> contour = new List<int>();



			return contour.ToArray();
		}

		public void UVMapPlane(Plane plane)
		{
			
		}
	}
}
