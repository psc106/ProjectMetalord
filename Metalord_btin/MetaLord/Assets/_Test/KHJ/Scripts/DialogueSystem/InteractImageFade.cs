using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InteractImageFade : MonoBehaviour
{
    public Transform playerTransform;
    public float distanceThreshold = 5f; // 조절하고자 하는 거리 임계값
    public Image image;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
        }
    }
    void Update()
    {
        FadeNpcInteractImage();
    }

    private void FadeNpcInteractImage()
    {
        if(playerTransform != null) 
        {
            // NPC와 플레이어 간의 거리 계산
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            // 거리에 따라 알파 값 조절
            float alphaValue = Mathf.InverseLerp(0f, distanceThreshold, distance);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alphaValue);
        }
    }

}
