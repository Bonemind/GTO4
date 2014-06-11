using UnityEngine;
using System.Collections.Generic;

public class QueueDrawer : MonoBehaviour
{
    /// <summary>
    /// The available prefabs (buildings) to place
    /// </summary>
    private List<ProductionStruct> Queue;

    private UnitProductionBuilding building;

    private Vector2 scrollPosition;

    public void Start()
    {
        building = gameObject.GetComponent<UnitProductionBuilding>();
        scrollPosition = Vector2.zero;
    }

    /// <summary>
    /// Draws the gui for available buildings.
    /// </summary>
    public void DrawGUI()
    {
        if (building == null)
        {
            return;
        }
        Queue = building.GetQueue();
        if (Queue.Count == 0)
        {
            return;
        }
        GUILayout.BeginArea(new Rect(Screen.width - 0.15f * Screen.width, 250, Screen.width * 0.15f, 0.6f * Screen.height));
        GUILayout.FlexibleSpace();
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
            for (int i = 0; i < Queue.Count; i++)
            {
                string name = Utils.RemoveClone(Queue[i].productionObject.name);
                string label = string.Format("Turns left: {0}", Queue[i].turnsLeft);
                RenderTexture texture;
                PrefabRenderer.RenderedTextures.TryGetValue(name, out texture);
                GUIContent gc;
                if (texture == null)
                {
                    label = string.Format("Unit: {0} \n {1}", name, label);
                    gc = new GUIContent(label);
                }
                else
                {
                    gc = new GUIContent(label, texture);
                }
                
                if (GUILayout.Button(gc))
                {
                    SendMessage("QueueAction", i, SendMessageOptions.DontRequireReceiver);
                }
            }
            GUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    }
}
