using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovedObject : MonoBehaviour
{
    bool isGrabed = false;
    Rigidbody myRigid;
    MeshCollider myColid;
    List<MeshCollider> childColid = new List<MeshCollider>();
    public LayerMask layerMask;

    private void Awake()
    {
        myColid = GetComponent<MeshCollider>();

        layerMask = 1 << LayerMask.NameToLayer("Default") |
        1 << LayerMask.NameToLayer("NPC") |
        1 << LayerMask.NameToLayer("StaticObject") |
        1 << LayerMask.NameToLayer("MovedObject") |
        1 << LayerMask.NameToLayer("CatchObject");
    }

    public void InitParentMovedObject()
    {        
        myRigid = GetComponent<Rigidbody>();
        myRigid.mass = 1000f;

        childColid.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            childColid.Add(transform.GetChild(i).GetComponent<MeshCollider>());
        }

        layerMask = 1 << LayerMask.NameToLayer("Default") |
        1 << LayerMask.NameToLayer("NPC") |
        1 << LayerMask.NameToLayer("StaticObject") |
        1 << LayerMask.NameToLayer("MovedObject") |
        1 << LayerMask.NameToLayer("CatchObject");
    }

    private void Update()
    {
        if(myRigid)
        {
            if (myRigid.IsSleeping())
            {
                if(childColid.Count != 0)
                {
                    for (int j = 0; j < childColid.Count; j++)
                    {
                        childColid[j].convex = false;
                    }
                }
                else
                {
                    myColid.convex = false;
                }
                Destroy(myRigid);
                myRigid = null;
            }
        }
    }

    // 그랩한 물건이 이동형 오브젝트와 부딪힐때마다 물리력 행사 콜백

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("MovedObject"))
    //    {
    //        if(collision.gameObject.GetComponent<PaintTarget>().CheckPainted())
    //        {
    //            return;
    //        }

    //        if (collision.gameObject.GetComponent<OverapObject>() == null)
    //        {
    //            collision.gameObject.AddComponent<OverapObject>().InitOverap();
    //        }
    //    }
    //}

    // 충돌지점 본드 체크
    private void OnCollisionStay(Collision collision)
    {
        // 이미 본드 동작을 하는 오브젝트를 다시 그랩하면 그랩하는순간 충돌면을 체크하여 그랩 해제됨에 따라 상태를 제어할 bool값 추가
        if (isGrabed == true)
        {
            return;
        }

        // 충돌이 일어나는 지점을 모두 체크
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 dir = collision.contacts[i].normal;

            Ray ray = new Ray(collision.contacts[i].point + dir, -dir);

            if (PaintTarget.RayChannel(ray, 1.5f, layerMask) == 0 && collision.gameObject.GetComponent<Controller_Physics>() == null)
            {
                GameObject parentObj = null;
                //Debug.Log("지금 닿고 있는 물건 : " + collision.gameObject);

                switch(LayerMask.LayerToName(collision.transform.gameObject.layer))
                {
                    case "CatchObject":          
                        
                        if(transform.gameObject.layer == LayerMask.NameToLayer("CatchObject"))
                        {                            
                            GameObject[] careeObj = new GameObject[collision.transform.childCount];

                            for (int j = 0; j < careeObj.Length; j++)
                            {
                                careeObj[j] = transform.GetChild(j).gameObject;
                            }

                            for (int j = 0; j < careeObj.Length; j++)
                            {
                                careeObj[j].transform.parent = transform;
                            }

                        }
                        else
                        {
                            transform.parent = collision.transform;
                            myColid.convex = false;
                        }
                        Destroy(myRigid);
                        GrabGun.instance.CancelObj();
                        isGrabed = true;
                        break;
                    case "Default":       
                        if(transform.gameObject.layer == LayerMask.NameToLayer("CatchObject"))
                        {
                            //Vector3 pos = transform.position - collision.contacts[i].point;
                            //transform.position -= pos;
                            Destroy(myRigid);
                            for (int j = 0; j < childColid.Count; j++)
                            {
                                childColid[j].convex = false;
                            }                            
                        }
                        else
                        {
                            parentObj = new GameObject();
                            GunStateController.AddList(parentObj);
                            parentObj.layer = LayerMask.NameToLayer("CatchObject");
                            parentObj.transform.position = collision.contacts[i].point;
                            transform.parent = parentObj.transform;
                            Destroy(myRigid);
                            myColid.convex = false;

                        }
                        GrabGun.instance.CancelObj();
                        isGrabed = true;
                        break;
                    case "MovedObject":                        
                        if(collision.gameObject.transform.parent != null &&
                            collision.gameObject.transform.parent.transform.gameObject.layer ==
                            LayerMask.NameToLayer("CatchObject"))
                        {
                            
                            transform.parent = collision.gameObject.transform.parent;
                            //collision.gameObject.GetComponentInParent<CatchObject>().AddChild(myColid);
                        }
                        else if(transform.gameObject.layer == LayerMask.NameToLayer("CatchObject") &&
                            collision.gameObject.transform.parent.transform.gameObject.layer !=
                            LayerMask.NameToLayer("CatchObject"))
                        {
                            collision.transform.parent = transform;
                        }
                        else
                        {
                            parentObj = new GameObject();
                            GunStateController.AddList(parentObj);
                            parentObj.layer = LayerMask.NameToLayer("CatchObject");
                            parentObj.transform.position = collision.contacts[i].point;
                            transform.parent = parentObj.transform;
                            collision.transform.parent = parentObj.transform;

                        }

                        if(childColid.Count != 0)
                        {
                            for (int j = 0; j < childColid.Count; j++)
                            {
                                childColid[j].convex = false;                                
                            }                            
                            
                        }
                        else
                        {
                            myColid.convex = false;

                        }

                        Destroy(myRigid);
                        GrabGun.instance.CancelObj();
                        isGrabed = true;
                        break;
                }
            }
        }
    }

    public void ChangedState()
    {
        myRigid = GetComponent<Rigidbody>();
        myRigid.mass = 1000f;
        myColid.material.dynamicFriction = 0f;
        myColid.material.bounciness = 0f;
        isGrabed = true;
    }
    
    public void CelarBond()
    {
        if (transform.parent != null)
        {
            myColid.convex = true;                          
            myRigid = transform.AddComponent<Rigidbody>();
            myRigid.mass = 1000f;
            myRigid.useGravity = true;
        }
    }

    public void StateChagned()
    {
        isGrabed = false;
    }

}
