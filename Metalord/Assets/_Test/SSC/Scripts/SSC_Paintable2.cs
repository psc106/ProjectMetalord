using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSC_Paintable2 : MonoBehaviour
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 2048);

    // Start is called before the first frame update
    void Start()
    {
        Renderer myRenderer = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);

        myRenderer.material.mainTexture = texture;
    }

    public void CleaningPaint()
    {
        Renderer myRenderer = GetComponent<Renderer>();

        myRenderer.material.mainTexture = texture;
    }

}
