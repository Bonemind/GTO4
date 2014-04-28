using UnityEngine;
using System.Collections;

public class TileControl : MonoBehaviour {
    private Color originalColor;
    public void start()
    {
        originalColor = transform.renderer.material.color;
    }

    public void OnMouseEnter()
    {
        transform.renderer.material.color = Color.cyan;
    }
    public void OnMouseExit()
    {
        transform.renderer.material.color = originalColor;
    }
}
