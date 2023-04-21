using UnityEngine;
using UnityEditor;

namespace SpaceApe.HierarchyToggle
{
    [InitializeOnLoad]
    public class ToggleActiveInHierarchy
    {
        static ToggleActiveInHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItem_CB;
        }

        private static void HierarchyWindowItem_CB(int selectionID, Rect selectionRect)
        {
            var gameObject = EditorUtility.InstanceIDToObject(selectionID) as GameObject;

            // Do everything except draw this magic, invisible toggle box
            if (gameObject != null && Event.current.type != EventType.Repaint)
            {
                Rect rect = new Rect(selectionRect);
                // Resize as square, to cover the hierarchy icon, but not the name
                rect.width = rect.height;
                bool active = GUI.Toggle(rect, gameObject.activeSelf, "");
                if (active != gameObject.activeSelf)
                {
                    Undo.RecordObject(gameObject, "Toggle GameObject Active");
                    gameObject.SetActive(active);
                    EditorUtility.SetDirty(gameObject);
                }
            }
        }
    }
}