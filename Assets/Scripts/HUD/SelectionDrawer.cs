using UnityEngine;
using System.Collections;

public class SelectionDrawer : MonoBehaviour {
    /// <summary>
    /// The available prefabs (buildings) to place
    /// </summary>
    public GameObject[] prefabs;

    /// <summary>
    /// Draws the gui for available buildings.
    /// </summary>
    public void DrawGUI()
    {
        GUILayout.BeginArea(new Rect(0, Screen.height - 0.1f * Screen.height, Screen.width, 0.1f * Screen.height));
        GUILayout.FlexibleSpace();
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int i = 0; i < prefabs.Length; i++)
            {
                string name = prefabs[i].name;
                RenderTexture texture;
                PrefabRenderer.RenderedTextures.TryGetValue(name, out texture);
                if (texture == null)
                {
                    if (GUILayout.Button(name))
                    {
                        SendMessage("HUDAction", prefabs[i], SendMessageOptions.DontRequireReceiver);
                    }
                }
                else
                {
                    if (GUILayout.Button(texture))
                    {
                        SendMessage("HUDAction", prefabs[i], SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    }
}
