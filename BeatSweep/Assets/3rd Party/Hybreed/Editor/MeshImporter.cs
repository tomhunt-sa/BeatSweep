using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Hybreed;

[System.Serializable]
public class MeshImporterWindow : EditorWindow
{
	public float voxelSize = 0.1f;
	public float scale = 4.0f;

	public MeshTools.AlignAxisType alignX = MeshTools.AlignAxisType.Center;
	public MeshTools.AlignAxisType alignY = MeshTools.AlignAxisType.None;
	public MeshTools.AlignAxisType alignZ = MeshTools.AlignAxisType.Min;

    public bool weldVertices = false;
    float _weldThreshold = 0.001f;
    float _weldBucketStep = 0.1f;

    public PicaVoxel.MeshingMode meshMode = PicaVoxel.MeshingMode.Greedy;
	public bool useConvexCollider = true;	
	public Material material = null;	
	
	[SerializeField]
	private string _materialPath = "Assets/Materials/VoxelObjects.mat";
	
	[SerializeField]
	private SerializedProperty _materialProp;
	
	[MenuItem("Import/Import Mesh Tools")]
	private static void DoImportImage()
	{
		MeshImporterWindow window = (MeshImporterWindow)EditorWindow.GetWindowWithRect((typeof(MeshImporterWindow)), new Rect(100, 100, 400, 220), true);
		window.Init();
	}
	
	/*
	 * Initialiser
	 */
	public void Init()
	{
		SerializedObject m_SerObj = new SerializedObject (this);
		
		titleContent = new GUIContent("MagicaVoxel Import");
		material = (Material) AssetDatabase.LoadAssetAtPath(_materialPath, typeof (Material));		
		
		_materialProp = m_SerObj.FindProperty ("material");
		_materialProp.objectReferenceValue = material;
	}
	
	/*
	 * Draw GUI contents
	 */
	public void OnGUI()
	{
		
		voxelSize = EditorGUILayout.FloatField("Voxel size: ", voxelSize);

		alignX = (MeshTools.AlignAxisType)EditorGUILayout.EnumPopup("AlignX: ", alignX);
		alignY = (MeshTools.AlignAxisType)EditorGUILayout.EnumPopup("AlignY: ", alignY);
		alignZ = (MeshTools.AlignAxisType)EditorGUILayout.EnumPopup("AlignZ: ", alignZ);

        weldVertices = EditorGUILayout.Toggle("Weld Vertices: ", weldVertices);

        meshMode = (PicaVoxel.MeshingMode)EditorGUILayout.EnumPopup("Mesh Mode: ", meshMode);

//		EditorGUILayout.PropertyField(_materialProp, new GUIContent("Material"));
//		material = (Material)_materialProp.objectReferenceValue;
        				
		EditorGUILayout.Space();
		if (GUILayout.Button("Import Selected .vox files"))
		{		
			// Import the files
			List<GameObject> importedObjects = ImportSelectedVox();
			
			// Select imported objects
			Object[] objects = new Object[importedObjects.Count];
			for(int i = 0 ; i < importedObjects.Count ; i++)
			{
				objects[i] = importedObjects[i];

				// Rotate on import
				Hybreed.MeshTools.RotateMesh(importedObjects[i]);
			}
			Selection.objects = objects;
		}
		
		EditorGUILayout.Space();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Center Mesh"))
		{
			for(int i = 0 ; i < Selection.gameObjects.Length ; i++)
			{
				Hybreed.MeshTools.CenterMesh(Selection.gameObjects[i], alignX, alignY, alignZ);
			}
		}
		
		if (GUILayout.Button("Rotate 90°"))
		{
			for(int i = 0 ; i < Selection.gameObjects.Length ; i++)
			{
				Hybreed.MeshTools.RotateMesh(Selection.gameObjects[i]);
			}
		}
        if (GUILayout.Button("Weld Vertices"))
        {
            List<Mesh> meshes = new List<Mesh>();
            GetSelectedMeshes(meshes);
            for(int i = 0 ; i < meshes.Count ; i++)
            {
                Hybreed.MeshTools.WeldMesh(meshes[i], _weldThreshold, _weldBucketStep);
            }
        }
        if (GUILayout.Button("Calc Normals"))
        {
            List<Mesh> meshes = new List<Mesh>();
            GetSelectedMeshes(meshes);
            for(int i = 0 ; i < meshes.Count ; i++)
            {
                meshes[i].RecalculateNormals();
            }
        }

        if (GUILayout.Button("Make UVs"))
        {
            List<Mesh> meshes = new List<Mesh>();
            GetSelectedMeshes(meshes);
            List<Vector2> uvs = new List<Vector2>();
            for(int i = 0 ; i < meshes.Count ; i++)
            {
                uvs.Clear();
                for(int j = 0 ; j < meshes[i].colors.Length ; j++)
                    uvs.Add(Vector2.zero);

                meshes[i].SetUVs(0, uvs);
            }
        }
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		scale = EditorGUILayout.FloatField("Scale: ", scale);
		if (GUILayout.Button("Scale"))
		{
            List<Mesh> meshes = new List<Mesh>();
            GetSelectedMeshes(meshes);
            for(int i = 0 ; i < meshes.Count ; i++)
            {
                Hybreed.MeshTools.ScaleMesh(meshes[i], scale);
            }
		}
		GUILayout.EndHorizontal();
	}

    /*
     * Get list of selected meshes in scene
     */
    public void GetSelectedMeshes(List<Mesh> meshes)
    {
        for(int i = 0 ; i < Selection.gameObjects.Length ; i++)
        {
            MeshFilter meshFilter = Selection.gameObjects[i].GetComponent<MeshFilter>();
            if (meshFilter && meshFilter.sharedMesh != null)
            {
                meshes.Add(meshFilter.sharedMesh);
            }
        }

        // Iterate through selected assets
		for(int i = 0 ; i < Selection.assetGUIDs.Length ; i++)
        {
			string assetPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[i]);
			Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
			if (mesh != null)
            {
				meshes.Add(mesh);
            }
        }
    }

	/*
	 * Import all selected .vox object
	 */
	public List<GameObject> ImportSelectedVox()
	{
		List<GameObject> importedObjects = new List<GameObject>();
		
		// Iterate through selected objects in Project tab
		for (int i = 0; i < Selection.objects.Length; ++i)
		{
			Object voxObj = Selection.objects[i];
			string path = AssetDatabase.GetAssetPath(voxObj);
			
			Debug.Log("Import " + path);
			GameObject meshObj = ImportVox(path);
			importedObjects.Add(meshObj);
		}

		return importedObjects;		
	}
	
	/*
	 * Import specified .vox object
	 */
	public GameObject ImportVox(string path)
	{
		string objectName = System.IO.Path.GetFileNameWithoutExtension(path);
        GameObject voxelObj = PicaVoxel.MagicaVoxelImporter.MagicaVoxelImport(path, objectName, voxelSize, true, meshMode, 1);
		
		// Create a new gameobject and combine the squillions of meshes into 1 (or more if lots of verts)
		GameObject meshObject = new GameObject(voxelObj.name);
		CombineMeshes.combineMeshes(voxelObj, meshObject);
		
		// Finished with Picavoxel object		
		GameObject.DestroyImmediate(voxelObj);	
		
		// Center mesh around origin
        Hybreed.MeshTools.CenterMesh(meshObject, alignX, alignY, alignZ);
		
		// Assign material
		MeshRenderer[] renderers = meshObject.GetComponentsInChildren<MeshRenderer>();
		for(int i = 0 ; i < renderers.Length ; i++)
			renderers[i].material = material;

		// Convex collisions
		MeshCollider[] colliders = meshObject.GetComponentsInChildren<MeshCollider>();
		foreach (var collider in colliders)
		{
			collider.convex = useConvexCollider;
			collider.enabled = false;
			collider.enabled = true;
		}

		return meshObject;
	}
    
	public static void RecalculateTangents(Mesh mesh)
	{
		//speed up math by copying the mesh arrays
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		
		//variable definitions
		int triangleCount = triangles.Length;
		int vertexCount = vertices.Length;
		
		Vector3[] tan1 = new Vector3[vertexCount];
		Vector3[] tan2 = new Vector3[vertexCount];
		
		Vector4[] tangents = new Vector4[vertexCount];
		
		for (long a = 0; a < triangleCount; a += 3)
		{
			long i1 = triangles[a + 0];
			long i2 = triangles[a + 1];
			long i3 = triangles[a + 2];
			
			Vector3 v1 = vertices[i1];
			Vector3 v2 = vertices[i2];
			Vector3 v3 = vertices[i3];
			
			Vector2 w1 = uv[i1];
			Vector2 w2 = uv[i2];
			Vector2 w3 = uv[i3];
			
			float x1 = v2.x - v1.x;
			float x2 = v3.x - v1.x;
			float y1 = v2.y - v1.y;
			float y2 = v3.y - v1.y;
			float z1 = v2.z - v1.z;
			float z2 = v3.z - v1.z;
			
			float s1 = w2.x - w1.x;
			float s2 = w3.x - w1.x;
			float t1 = w2.y - w1.y;
			float t2 = w3.y - w1.y;
						
			float div = s1 * t2 - s2 * t1;
			float r = div == 0.0f ? 0.0f : 1.0f / div;
			
			Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
			
			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;
			
			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;
		}
		
		
		for (long a = 0; a < vertexCount; ++a)
		{
			Vector3 n = normals[a];
			Vector3 t = tan1[a];
			
			//Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
			//tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
			Vector3.OrthoNormalize(ref n, ref t);
			tangents[a].x = t.x;
			tangents[a].y = t.y;
			tangents[a].z = t.z;
			
			tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
		}
		
		mesh.tangents = tangents;
	}
    
}