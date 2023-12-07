using UnityEngine;

public class TestPaintCheck : MonoBehaviour
{
    public Camera cam;
    public Renderer sprite;

    Texture2D texture;

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            TestGetPixel();
        }
    }

    private void TestGetPixel()
    {
        Vector3 viewPos = Input.mousePosition;

        texture = RTImage(cam);
        Color _color = texture.GetPixel((int)viewPos.x, (int)viewPos.y);
        Debug.Log(_color);
        sprite.material.color = _color;
    }

    Texture2D RTImage(Camera cam)
    {
        // 사용할 RenderTexture를 먼저 생성
        RenderTexture renderTexture = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0);
        // 카메라의 targetTexture를 생성한 RenderTexture로 지정
        cam.targetTexture = renderTexture;
        // 렌더 텍스처로 렌더링
        cam.Render();

        // 현재 활성화된 RenderTexture를 가져오고 설정
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;

        // RenderTexture에서 픽셀을 읽어 Texture2D로 복사
        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();

        // 원래의 RenderTexture를 복구
        RenderTexture.active = currentRT;

        return image;
    }
}
