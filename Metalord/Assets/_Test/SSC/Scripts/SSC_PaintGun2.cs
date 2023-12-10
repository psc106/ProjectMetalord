using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class SSC_PaintGun2 : MonoBehaviour
{
    [SerializeField] private int penSize = 5;

    public Renderer _renderer;
    private SSC_Paintable2 _Paintable2;
    private Vector2 _targetPos, lastPos;
    private Color[] penColor;

    // Start is called before the first frame update
    void Start()
    {        
        penColor = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            DrawPaint();
        }
    }

    public void DrawPaint()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.GetComponent<SSC_Paintable2>() != null)
            {
                _Paintable2 = hit.transform.GetComponent<SSC_Paintable2>();

                _targetPos = new Vector2(hit.textureCoord.x, hit.textureCoord.y);

                var x = (int)(_targetPos.x * _Paintable2.textureSize.x - (penSize * 0.5f));
                var y = (int)(_targetPos.y * _Paintable2.textureSize.y - (penSize * 0.5f));

                _Paintable2.texture.SetPixels(x, y, penSize, penSize, penColor);

                //for(float f = 0.01f; f < 1.00f; f += 0.03f)
                //{
                //    var lerpX = (int)Mathf.Lerp(lastPos.x, x, f);
                //    var lerpY = (int)Mathf.Lerp(lastPos.y, y, f);
                //    _Paintable2.texture.SetPixels(lerpX, lerpY, penSize, penSize, penColor);
                //}

                _Paintable2.texture.Apply();

                //lastPos = new Vector2(x, y);
            }
        }
    }
}
