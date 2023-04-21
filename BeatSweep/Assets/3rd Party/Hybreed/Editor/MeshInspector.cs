using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MeshFilter))]
public class MeshInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MeshFilter meshFilter = (MeshFilter)this.target;
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            GUILayout.Label("Vertex Count: " + meshFilter.sharedMesh.vertexCount.ToString("n0"));
//			GUILayout.Label("Sub Mesh Count: " + meshFilter.sharedMesh.subMeshCount);
			for( int i = 0 ; i < meshFilter.sharedMesh.subMeshCount ; i++)
			{
				int triCount = meshFilter.sharedMesh.GetIndices(i).Length / 3;
				GUILayout.Label(" Sub Mesh " + i + " Tri Count: " + triCount.ToString("n0"));
			}
        }
    }
}