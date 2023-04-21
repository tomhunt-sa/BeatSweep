using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Hybreed
{
	public class AssetTools
	{
		/*
		 * If item exists in asset database, reflect changes on disk
		 */
		public static T UpdateAsset<T>(T asset) where T : UnityEngine.Object
		{
			// Save changes to asset database
			string path = AssetDatabase.GetAssetPath(asset);

			// Bail if not in asset database
			if (path == null || path.Length == 0)
				return null;

			// Save changes to asset database
			return UpdateAsset<T>(asset, path);
		}

		/*
		 * Update or create asset in asset database
		 */
        public static T UpdateAsset<T>(T asset, string path) where T : UnityEngine.Object
		{            
			// Load original asset
			T originalItem = AssetDatabase.LoadAssetAtPath<T>(path);
            if (originalItem == null)
            {
                // Create new asset
                AssetDatabase.CreateAsset(asset, path);
            }
            else if (originalItem != asset)
			{
                object []temp = AssetDatabase.LoadAllAssetsAtPath(path);
                Debug.Log(temp.Length + " objects in " + path);

                if (originalItem is Mesh)
                {
                    Mesh srcMesh = (asset as Mesh);
                    Mesh dstMesh = (originalItem as Mesh);

                    dstMesh.Clear();
                    dstMesh.subMeshCount = srcMesh.subMeshCount;
                    dstMesh.vertices = srcMesh.vertices;
                    dstMesh.uv = srcMesh.uv;
                    dstMesh.colors = srcMesh.colors;
                    for(int i = 0 ; i < srcMesh.subMeshCount ; i++)
                        dstMesh.SetTriangles(srcMesh.GetTriangles(i), i);
                    dstMesh.bounds = srcMesh.bounds;
                    dstMesh.normals = srcMesh.normals;
                }
                else
                {
                    EditorUtility.CopySerialized(asset, originalItem);
                    EditorUtility.SetDirty(originalItem);
                }
	        }
			
			AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(); 

            // Load asset back from disk
            originalItem = AssetDatabase.LoadAssetAtPath<T>(path);
            			      
            return originalItem;
		}
	}
}
