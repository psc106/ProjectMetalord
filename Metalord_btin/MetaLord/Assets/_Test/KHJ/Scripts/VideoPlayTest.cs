using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayTest : MonoBehaviour
{
    public GameObject test;

    void Start()
    {
        test.SetActive(false);
    }

    public void TestPlayVideo()
    {
        test.SetActive(true);
    }

    public void OffCanvas()
    {
        gameObject.SetActive(false);
    }
}
