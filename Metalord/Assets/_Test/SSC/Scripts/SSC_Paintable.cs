using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSC_Paintable : MonoBehaviour
{
    public int RESOLUTION_X = 512;
    public int RESOLUTION_Y = 512;


    private Texture2D mainTex;
    private MeshRenderer mr;
    private RenderTexture rt;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CleaningTexture();
        }
    }

    private void Awake()
    {
        TryGetComponent(out mr);
        rt = new RenderTexture(RESOLUTION_X, RESOLUTION_Y, 32);

        if (mr.material.mainTexture != null)
        {
            mainTex = mr.material.mainTexture as Texture2D;
        }
        // 메인 텍스쳐가 없을 경우, 하얀 텍스쳐를 생성하여 사용
        else
        {
            mainTex = new Texture2D(RESOLUTION_X, RESOLUTION_Y);
        }

        // 메인 텍스쳐 -> 렌더 텍스쳐 복제
        Graphics.Blit(mainTex, rt);

        // 렌더 텍스쳐를 메인 텍스쳐에 등록
        mr.material.mainTexture = rt;
    }

    public void DrawTexture(Vector2 uv, float brushSize, Texture2D brushTexture)
    {
        uv.x *= RESOLUTION_X;
        uv.y *= RESOLUTION_Y;
        RenderTexture.active = rt; // 페인팅을 위해 활성 렌더 텍스쳐 임시 할당
        GL.PushMatrix();                                  // 매트릭스 백업
        GL.LoadPixelMatrix(0, RESOLUTION_X, RESOLUTION_Y, 0); // 알맞은 크기로 픽셀 매트릭스 설정

        float brushPixelSize = brushSize * RESOLUTION_X;

        // 렌더 텍스쳐에 브러시 텍스쳐를 이용해 그리기
        Graphics.DrawTexture(new Rect
            (uv.x - brushPixelSize * 0.5f,
            (rt.height - uv.y) - brushPixelSize * 0.5f,
            brushPixelSize,
            brushPixelSize
            ),
            brushTexture
        );

        GL.PopMatrix();              // 매트릭스 복구
        RenderTexture.active = null; // 활성 렌더 텍스쳐 해제
    }

    public void CleaningTexture()
    {
        Graphics.Blit(mainTex, rt);
        mr.material.mainTexture = rt;
    }
}
