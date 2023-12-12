using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigController : MonoBehaviour
{
    [SerializeField]
    Controller_Physics player;

    [SerializeField] Transform startPoint;
    [SerializeField] Transform cameraPoint;
    [SerializeField] Transform target;
    [SerializeField] LayerMask layer;

    [SerializeField] float range = 50;

    [SerializeField] Rig aimRig;

    private void Start()
    {
        cameraPoint = Camera.main.transform;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (player.OnClimb)
        {
            aimRig.weight = 0;
        }
        else
        {
            aimRig.weight = 1;
        }

        target.position = cameraPoint.position + cameraPoint.forward * range;

        RaycastHit hit;
        if(Physics.Raycast(startPoint.position, target.position-startPoint.position, out hit, range, layer))
        {
            target.position = hit.point;
        }
    }
}
