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
    [SerializeField] Transform aimTarget;
    [SerializeField] Transform rotateTarget;
    [SerializeField] LayerMask layer;

    [SerializeField] float range = 50;

    [SerializeField] Rig aimRig;
    [SerializeField] Rig rotateRig;

    RaycastHit hit;

    private void Start()
    {
        cameraPoint = Camera.main.transform;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        aimRig.weight = player.OnClimb ? 0 : 1;
        rotateRig.weight = player.OnClimb ? 1 : 0;

        if (player.OnClimb)
        {
            rotateTarget.rotation = Quaternion.LookRotation(-player.GetClimbNormal());
        }

        aimTarget.position = cameraPoint.position + cameraPoint.forward * range;
        if(Physics.Raycast(startPoint.position, aimTarget.position-startPoint.position, out hit, range, layer))
        {
            aimTarget.position = hit.point;
        }
    }
}
