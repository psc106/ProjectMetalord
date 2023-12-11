using Cinemachine.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class RigController : MonoBehaviour
{
    [SerializeField]
    Controller_Physics player;

    [SerializeField] Transform oriented;
    [SerializeField] Transform headPoint;
    [SerializeField] Transform legPoint;

    private void Awake()
    {
    }
    private void Start()
    {
        headPoint.position = player.transform.position + (oriented.forward + Vector3.up) * 5;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        headPoint.position = player.transform.position + (oriented.forward + Vector3.up) * 5;
    }
}
