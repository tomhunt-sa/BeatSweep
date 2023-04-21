using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MCVoxImporter
{
    /*
     * Import a .vox file
     */
    static public MeshLODGroup ImportVox(string filename, string outMeshDirectory, float[] lodScales, float voxelSize)
    {
        int lodCount = lodScales.Length;

        PicaVoxel.MeshingMode meshMode = PicaVoxel.MeshingMode.Greedy;
        Debug.Log("MCVoxImporter: Importing " + filename);

        // Create mesh LOD group
        MeshLODGroup meshLODGroup = ScriptableObject.CreateInstance<MeshLODGroup>();
        meshLODGroup.hideFlags = HideFlags.DontUnloadUnusedAsset;

        float importVoxelSize = 0.1f;
        string filenameNoExt = Path.GetFileNameWithoutExtension(filename);

        List<Mesh> meshes = new List<Mesh>();

		for(int i = 0 ; i < lodCount ; i++)
		{
            GameObject voxelObj = PicaVoxel.MagicaVoxelImporter.MagicaVoxelImport(filename, filenameNoExt, importVoxelSize, true, meshMode, lodScales[i]);
		
	        string assetFilename = outMeshDirectory + Path.DirectorySeparatorChar + filenameNoExt + "_LOD" + i + ".asset";
	        Debug.Log("Combining mesh: " + assetFilename);

	        // Create a new GameObject and combine the squillions of meshes into 1 (or more if lots of verts)
	        GameObject meshObject = new GameObject(voxelObj.name);
	        CombineMeshes.combineMeshes(voxelObj, meshObject, assetFilename);

	        Mesh mesh = meshObject.GetComponent<MeshFilter>().sharedMesh;

			// Store LOD mesh
	        mesh = Hybreed.AssetTools.UpdateAsset<Mesh>(mesh, assetFilename);
	        meshLODGroup.MeshLODs.Add(mesh);

			// Finished with GameObjects
	        GameObject.DestroyImmediate(voxelObj);  
	        GameObject.DestroyImmediate(meshObject);

            // Center mesh on X & Z axis
            mesh = Hybreed.MeshTools.CenterMesh(mesh, Hybreed.MeshTools.AlignAxisType.Center, Hybreed.MeshTools.AlignAxisType.Min, Hybreed.MeshTools.AlignAxisType.Center);

            // Compress mesh
			MeshUtility.SetMeshCompression(mesh, ModelImporterMeshCompression.High);
			mesh = Hybreed.AssetTools.UpdateAsset<Mesh>(mesh, assetFilename);

			meshes.Add(mesh);
		}
        
        // Rotate by 90 degrees
        Hybreed.MeshTools.RotateMeshes(meshes);

		// Scale main mesh to required size
        Hybreed.MeshTools.ScaleMeshes(meshes, voxelSize / importVoxelSize);

//	        if ( i > 0)
//	        {
//	        	int lod = i - 1;
//
//				float weldThreshold = voxelSize * 0.2f;
//		        float weldIncrement = voxelSize * 4.0f;
//
//	            // Use WeldMesh to reduce vertex count
//				float threshold = weldThreshold + lod * weldIncrement;
//	            float bucketStep = 1.0f;
//				mesh = Hybreed.MeshTools.WeldMesh(mesh, threshold, bucketStep, false);
//	              
//	            // Use LODMaker to reduce triangle count
//				mesh = LODMaker.MakeLODMesh(mesh, 0.0f, false, 0.0f, true);
//            }
//        }
                                
//        float weldThreshold = voxelSize * 0.2f;
//        float weldIncrement = voxelSize * 4.0f;
//
//        for (int i = 0 ; i < lodCount ; i++)
//        {
//            // Use WeldMesh to reduce vertex count
//            float threshold = weldThreshold + i * weldIncrement;
//            float bucketStep = 1.0f;
//            Mesh meshLOD = Hybreed.MeshTools.WeldMesh(mesh, threshold, bucketStep, false);
//              
//            // Use LODMaker to reduce triangle count
//            meshLOD = LODMaker.MakeLODMesh(meshLOD, 0.0f, false, 0.0f, true);
//
//            // Scale to required size
//            Hybreed.MeshTools.ScaleMesh(meshLOD, voxelSize / importVoxelSize);
//
//            // Persist mesh to disk
//            string lodMeshFilename = outMeshDirectory + Path.DirectorySeparatorChar + filenameNoExt + "_LOD" + (i + 1) + ".asset";
//            meshLOD = Hybreed.AssetTools.UpdateAsset<Mesh>(meshLOD, lodMeshFilename);
//
//            meshLODGroup.MeshLODs.Add(meshLOD);
//        }


        // Save new mesh LOD group to asset database
        string meshLODGroupFilename = outMeshDirectory + Path.DirectorySeparatorChar + filenameNoExt + ".asset";
        meshLODGroup = Hybreed.AssetTools.UpdateAsset<MeshLODGroup>(meshLODGroup, meshLODGroupFilename);

        meshLODGroup.hideFlags = HideFlags.None;

        return meshLODGroup;
    }
	
}
