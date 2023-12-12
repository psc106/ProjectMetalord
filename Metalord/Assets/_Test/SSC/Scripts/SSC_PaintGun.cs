using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSC_PaintGun : MonoBehaviour
{
    const int RESOLUTION = 512;

    //[Range(0.01f, 1f)]
    public float brushSize = 0.0001f;
    public Texture2D brushTexture;

    List<SSC_Paintable> paintList = new List<SSC_Paintable>();

    private void Awake()
    {
        // 브러시 텍스쳐가 없을 경우 임시 생성(red 색상)
        if (brushTexture == null)
        {
            brushTexture = new Texture2D(RESOLUTION, RESOLUTION);
            for (int i = 0; i < RESOLUTION; i++)
                for (int j = 0; j < RESOLUTION; j++)
                    brushTexture.SetPixel(i, j, Color.red);
            brushTexture.Apply();
        }
    }

    private void Update()
    {
        // 마우스 클릭 지점에 브러시로 그리기
        if (Input.GetMouseButton(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            Physics.Raycast(ray, out var hit);

            if (hit.transform.GetComponent<SSC_Paintable>() != null)
            {                
                Vector2 pixelUV = hit.textureCoord;
                //Debug.Log($"찍히는 좌표 : {hit.lightmapCoord}");
                //Debug.Log($"tex1: {hit.textureCoord}");
                //Debug.Log($"tex2: {hit.textureCoord2}");

                //pixelUV *= RESOLUTION;
                //Debug.Log(pixelUV);
                hit.transform.GetComponent<SSC_Paintable>().DrawTexture(pixelUV, brushSize, brushTexture);
            }
        }

    }
}
