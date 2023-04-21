using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hybreed
{
	public class MeshTools
	{
		/*
		 * Replace existing vertices in a mesh
		 */
        public static Mesh ReplaceMeshVertices(Mesh mesh, Vector3[] vertices)
	    {
			// Assign new vertex positions
			mesh.vertices = vertices;
			
			// Update mesh attributes
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			mesh.Optimize();

			// Persist any changes to existing asset
			return Hybreed.AssetTools.UpdateAsset<Mesh>(mesh);
	    }

		public enum AlignAxisType
		{
			Min,
			Max,
			Center,
			None
		}

		/*
		 * Center contents of mesh around origin
		 */
		public static Mesh CenterMesh(Mesh mesh, AlignAxisType alignX, AlignAxisType alignY, AlignAxisType alignZ)
		{			
			List<Mesh> meshes = new List<Mesh>();
			meshes.Add(mesh);
			CenterMeshes(meshes, alignX, alignY, alignZ);
            return meshes[0];
		}

		/*
		 * Center contents of mesh around origin
		 */
		public static void CenterMesh(GameObject gameObject, AlignAxisType alignX, AlignAxisType alignY, AlignAxisType alignZ)
		{
			List<MeshFilter> meshFilters = new List<MeshFilter>();
			gameObject.GetComponentsInChildren<MeshFilter>(meshFilters);

			List<Mesh> meshes = new List<Mesh>();

			for (int i = 0 ; i < meshFilters.Count ; i++)
			{
				meshes.Add(meshFilters[i].sharedMesh);
			}

			CenterMeshes(meshes, alignX, alignY, alignZ);
		}

		/*
		 * Center contents of mesh around origin
		 */
		public static void CenterMeshes(List<Mesh> meshes, AlignAxisType alignX, AlignAxisType alignY, AlignAxisType alignZ)
		{			
			// Generate bounds for all vertices
			bool first = true;
			Bounds bounds = new Bounds();
			for(int i = 0 ; i < meshes.Count ; i++)
			{
				Mesh mesh = meshes[i];
				if (mesh == null)
					continue;

				Vector3[] vertices = mesh.vertices;
				
				for(int j = 0 ; j < vertices.Length ; j++)
				{
					if (first)
					{
						bounds.center = vertices[j];
						first = false;
					}
					else
						bounds.Encapsulate(vertices[j]);				
				}
			}
		
			// Offset all vertices by center of bounds
			for(int i = 0 ; i < meshes.Count ; i++)
			{
				Mesh mesh = meshes[i];
				if (mesh == null)
					continue;
				
				Vector3[] vertices = mesh.vertices;
				
				for(int j = 0 ; j < vertices.Length ; j++)
				{
					if (alignX == AlignAxisType.Center)
						vertices[j].x -= bounds.center.x;
					else if (alignX == AlignAxisType.Min)
						vertices[j].x -= bounds.min.x;
					else if (alignX == AlignAxisType.Min)
						vertices[j].x -= bounds.max.x;

					if (alignY == AlignAxisType.Center)
						vertices[j].y -= bounds.center.y;
					else if (alignY == AlignAxisType.Min)
						vertices[j].y -= bounds.min.y;
					else if (alignY == AlignAxisType.Min)
						vertices[j].y -= bounds.max.y;

					if (alignZ == AlignAxisType.Center)
						vertices[j].z -= bounds.center.z;
					else if (alignZ == AlignAxisType.Min)
						vertices[j].z -= bounds.min.z;
					else if (alignZ == AlignAxisType.Min)
						vertices[j].z -= bounds.max.z;
				}
				
				// Assign new vertex positions
                meshes[i] = ReplaceMeshVertices(mesh, vertices);
			}
		}

		/*
		 * Rotate mesh elements by 90 degrees
		 */
		public static void RotateMesh(GameObject gameObject)
		{
			List<MeshFilter> meshFilters = new List<MeshFilter>();
			gameObject.GetComponentsInChildren<MeshFilter>(meshFilters);

			for (int i = 0 ; i < meshFilters.Count ; i++)
			{
				if (meshFilters[i].sharedMesh != null)
				{
					RotateMesh(meshFilters[i].sharedMesh);
				}
			}
		}

		/*
		 * Rotate a list of meshes by 90 degrees
		 */
		public static void RotateMeshes(List<Mesh> meshes)
		{
			for(int i = 0 ; i < meshes.Count ; i++)
				RotateMesh(meshes[i]);
		}

		/*
		 * Rotate mesh elements by 90 degrees
		 */
        public static Mesh RotateMesh(Mesh mesh)
		{
			Vector3[] vertices = mesh.vertices;

			// Rotate by 90 degrees			
			for(int j = 0 ; j < vertices.Length ; j++)
				vertices[j] = Quaternion.Euler(0, 90, 0) * vertices[j];
			
			// Assign new vertex positions
			return ReplaceMeshVertices(mesh, vertices);
		}

		/*
		 * Scale vertices in a list of meshes
		 */
		public static void ScaleMeshes(List<Mesh> meshes, float amount)
		{
			for(int i = 0 ; i < meshes.Count ; i++)
                meshes[i] = ScaleMesh(meshes[i], amount);
		}

        /*
         * Scale vertex positions in a mesh
         */
        public static Mesh ScaleMesh(Mesh mesh, float amount)
        {            
            Vector3[] vertices = mesh.vertices;
            
            for(int j = 0 ; j < vertices.Length ; j++)
            {
                vertices[j] = vertices[j] * amount;
            }
            
            // Assign new vertex positions
            return ReplaceMeshVertices(mesh, vertices);
        }

        /*
         * Removes and combine vertices when they're "close enough"
         */
        public static Mesh WeldMesh(Mesh srcMesh, float threshold, float bucketStep, bool reuseMesh = false)
        {
            Mesh dstMesh = null;

            // Overwrite input mesh if required
            if (reuseMesh == true)
                dstMesh = srcMesh;
             else
                dstMesh = new Mesh();

            Vector3[] oldVertices = srcMesh.vertices;
            Vector3[] newVertices = new Vector3[oldVertices.Length];

            Color[] oldColors = srcMesh.colors;
            List<Color> newColors = new List<Color>();

            Vector2[] oldUVs = srcMesh.uv;
            List<Vector2> newUVs = new List<Vector2>();


            int[] old2new = new int[oldVertices.Length];
            int newSize = 0;
     
            // Find AABB
            Vector3 min = new Vector3 (float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3 (float.MinValue, float.MinValue, float.MinValue);
            for (int i = 0; i < oldVertices.Length; i++)
            {
                if (oldVertices[i].x < min.x) min.x = oldVertices[i].x;
                if (oldVertices[i].y < min.y) min.y = oldVertices[i].y;
                if (oldVertices[i].z < min.z) min.z = oldVertices[i].z;
                if (oldVertices[i].x > max.x) max.x = oldVertices[i].x;
                if (oldVertices[i].y > max.y) max.y = oldVertices[i].y;
                if (oldVertices[i].z > max.z) max.z = oldVertices[i].z;
            }
     
            // Make cubic buckets, each with dimensions "bucketStep"
            int bucketSizeX = Mathf.FloorToInt ((max.x - min.x) / bucketStep) + 1;
            int bucketSizeY = Mathf.FloorToInt ((max.y - min.y) / bucketStep) + 1;
            int bucketSizeZ = Mathf.FloorToInt ((max.z - min.z) / bucketStep) + 1;
            List<int>[,,] buckets = new List<int>[bucketSizeX, bucketSizeY, bucketSizeZ];
     
            // Make new vertices
            for (int i = 0; i < oldVertices.Length; i++)
            {
                // Determine which bucket it belongs to
                int x = Mathf.FloorToInt ((oldVertices[i].x - min.x) / bucketStep);
                int y = Mathf.FloorToInt ((oldVertices[i].y - min.y) / bucketStep);
                int z = Mathf.FloorToInt ((oldVertices[i].z - min.z) / bucketStep);

                // Check to see if it's already been added
                if (buckets[x, y, z] == null)
                    buckets[x, y, z] = new List<int> (); // Make buckets lazily

                for (int j = 0; j < buckets[x, y, z].Count; j++)
                {
                    Vector3 to = newVertices[buckets[x, y, z][j]] - oldVertices[i];
                    if (Vector3.SqrMagnitude (to) < threshold)
                    {
                        int newIx = buckets[x, y, z][j];
                        if (oldColors[i] == newColors[newIx] && (oldUVs[i] - newUVs[newIx]).magnitude < 0.01f)
                        {
                            old2new[i] = buckets[x, y, z][j];
                            goto skip; // Skip to next old vertex if this one is already there
                        }
                    }
                }

                // Add new vertex
                newVertices[newSize] = oldVertices[i];
                newColors.Add(oldColors[i]);
                newUVs.Add(oldUVs[i]);
                buckets[x, y, z].Add (newSize);
                old2new[i] = newSize;
                newSize++;

                skip:;
            }
     
            // Make new triangles
            int[] oldTris = srcMesh.triangles;
            int[] newTris = new int[oldTris.Length];
            for (int i = 0; i < oldTris.Length; i++)
                newTris[i] = old2new[oldTris[i]];
         
            Vector3[] finalVertices = new Vector3[newSize];
            for (int i = 0; i < newSize; i++)
                finalVertices[i] = newVertices[i];

            // Assign to output mesh
            dstMesh.Clear();
            dstMesh.subMeshCount = 1;
            dstMesh.vertices = finalVertices;
            dstMesh.SetColors(newColors);
            dstMesh.SetUVs(0, newUVs);
            dstMesh.triangles = newTris;
            dstMesh.RecalculateNormals ();
            dstMesh.Optimize ();

            return dstMesh;
        }
	}
}