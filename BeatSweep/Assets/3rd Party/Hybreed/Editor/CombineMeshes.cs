using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CombineMeshes
{		
	public static void combineMeshes(GameObject srcObj, GameObject dstObj, string outFilename = null)
	{
		int meshNum = 0;
		int vertCount = 0;
		
		Material material = null;
		
		// Get all meshes in source object 
		MeshFilter[] meshFilters = srcObj.transform.GetComponentsInChildren<MeshFilter>();
		List<CombineInstance> combines = new List<CombineInstance>(meshFilters.Length);
		
		// Loop until all verts have been combined
		int meshIx = 0;
		while(meshIx != meshFilters.Length)
		{
			vertCount = 0;
		
			combines.Clear();
			for( ; meshIx < meshFilters.Length ; meshIx++)
			{			
				MeshFilter meshFilter = meshFilters[meshIx];
				if (meshFilter.sharedMesh == null)
					continue;
			
				// Bail if we've hit the vertex limit
				if (vertCount + meshFilter.sharedMesh.vertexCount > System.UInt16.MaxValue)
					break;
				
				if (material == null)
					material = meshFilters[meshIx].gameObject.GetComponent<MeshRenderer>().sharedMaterial;
										
				CombineInstance combine = new CombineInstance();
				combine.mesh = meshFilter.sharedMesh;
				combine.transform = meshFilter.transform.localToWorldMatrix;
				combines.Add(combine);
				
				vertCount += meshFilter.sharedMesh.vertexCount;			
	         }
		
			// Bail if there's nothing to do
			if (vertCount == 0)
				break;
		
			// Create new child for combined mesh
			GameObject childObj = null;
			
			if (meshNum == 0 && meshIx == meshFilters.Length)
			{
				// Mesh is contained to destination game object
				childObj = dstObj;
			}
			else
			{
				// Split meshes into multiple child objects
				childObj = new GameObject("Mesh " + (meshNum + 1));
				childObj.transform.SetParent(dstObj.transform, false);
			}

			string outDirectory;
			if (outFilename == null)
			{
	            // Build asset path
				outDirectory = "Assets" + Path.DirectorySeparatorChar + "Meshes" + Path.DirectorySeparatorChar + "CombinedMeshes";
				outFilename = outDirectory + Path.DirectorySeparatorChar + srcObj.name + meshNum + ".asset";
            }
            else
            {
				outDirectory = Path.GetDirectoryName(outFilename);
			}

            // Create combined mesh directory if required
			if (!Directory.Exists(outDirectory))
				Directory.CreateDirectory(outDirectory);

            // Attempt to load existing mesh asset
			Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(outFilename);

            if (mesh == null)
            {
    			// Create new mesh container
    			mesh = new Mesh();
				AssetDatabase.CreateAsset(mesh, outFilename);
            }
            else
            {
                // Clear contents of existing mesh
                mesh.Clear();
            }
			
			// Add to game object
			childObj.AddComponent<MeshFilter>().sharedMesh = mesh;
			
			//Zero transformation is needed because of localToWorldMatrix transform
			Vector3 position = childObj.transform.position;
			childObj.transform.position = Vector3.zero;
			
			// Perform the combine
			mesh.CombineMeshes(combines.ToArray(), true, true);
			mesh.Optimize();
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
		
			// Save combined mesh as a new asset
			AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
			//Reset position
			childObj.transform.position = position;
			
			//Adds collider to mesh		
			MeshCollider meshCollider = childObj.GetComponent<MeshCollider>();
			if (meshCollider == null)
				meshCollider = childObj.AddComponent<MeshCollider>();		
			meshCollider.sharedMesh = mesh; 
		
			MeshRenderer meshRenderer = childObj.GetComponent<MeshRenderer>();
			if (meshRenderer == null)
				meshRenderer = childObj.AddComponent<MeshRenderer>();
				
			meshRenderer.sharedMaterial = material;
			
			meshNum++;
		}		
	}
}