using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Container class to hold a set of meshes of decreasing complexity for use in LOD system
 */
[CreateAssetMenu]
public class MeshLODGroup : ScriptableObject
{
    [SerializeField]
    private List<Mesh> _meshLODs = new List<Mesh>();
    public List<Mesh> MeshLODs { get { return _meshLODs; } }
    public int MeshLODCount { get { return _meshLODs.Count; } }

    [SerializeField]
    int _sourceChecksum;
	public int SourceChecksum { get { return _sourceChecksum; } set { _sourceChecksum = value; } }

    /*
     * Returns a specific LOD by its index
     */
    public Mesh GetMeshLOD(int ix)
    {
        Mesh meshLOD = null;
        if (ix < _meshLODs.Count)
            meshLOD = _meshLODs[ix];

        return meshLOD;
    }

    /*
     * Remove all normals
     * Used for optimisation
     */
	public void ClearNormals()
	{
		for(int i = 0 ; i < _meshLODs.Count ; i++)
		{
			// Clear mesh LOD normals
			_meshLODs[i].normals = null;
		}
	}
}