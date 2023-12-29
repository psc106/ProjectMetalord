using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.TextCore.Text;
using UnityEngine;

public class MovedObject : MonoBehaviour
{
    bool checkContact = false;
    Rigidbody myRigid;
    MeshCollider myColid;    
    public LayerMask layerMask;

    private void Awake()
    {
        checkContact = false;
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
                Destroy(myRigid);
                myRigid = null;
                myColid.convex = false;
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
        if (checkContact == false)
        {
            return;
        }

        // 충돌이 일어나는 지점을 모두 체크
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 dir = collision.contacts[i].normal;

            Ray ray = new Ray(collision.contacts[i].point + dir, -dir);

            if (PaintTarget.RayChannel(ray, 1.5f, layerMask) == 0 && collision.gameObject.GetComponent<Controller_Physics>() == null && checkContact)
            {
                GameObject parentObj = null;    
                
                // 충돌한 오브젝트의 부모가 없을경우
                if(collision.transform.parent == null)
                {                    
                    // 상위 오브젝트 생성
                    parentObj = new GameObject();
                    parentObj.transform.gameObject.layer = LayerMask.NameToLayer("CatchObject");                    
                    CatchObject controll = parentObj.AddComponent<CatchObject>();
                    GunStateController.AddList(controll);
                    parentObj.transform.position = collision.contacts[i].point;

                    // 그랩한 오브젝트와 충돌한 오브젝트 모두 상위 오브젝트 종속, HashSet 갱신                    
                    transform.parent = parentObj.transform;
                    collision.transform.parent = parentObj.transform;
                    //GunStateController.AddList(collision.gameObject.GetComponent<MovedObject>());
                    controll.AddChild(transform.GetComponent<MeshCollider>());
                    controll.AddChild(collision.transform.GetComponent<MeshCollider>());                    
                }
                // 부모가 존재할 경우
                else
                {
                    GameObject contactObj = collision.gameObject;                    

                    // 부모가 상위 오브젝트 경우
                    if(contactObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("CatchObject"))
                    {                        
                        // HashSet 갱신    
                        transform.parent = contactObj.transform.parent;
                        contactObj.transform.parent.GetComponent<CatchObject>().AddChild(transform.GetComponent<MeshCollider>());        

                    }
                    // 부모가 고정형 오브젝트 경우
                    else if(contactObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("Default"))
                    {                        
                        // 상위 오브젝트 생성
                        parentObj = new GameObject();
                        parentObj.transform.gameObject.layer = LayerMask.NameToLayer("CatchObject");                        
                        CatchObject controll = parentObj.AddComponent<CatchObject>();                        
                        GunStateController.AddList(controll);
                        parentObj.transform.position = collision.contacts[i].point;

                        // 그랩한 오브젝트와 충돌한 오브젝트 모두 상위 오브젝트 종속, HashSet 갱신    
                        transform.parent = parentObj.transform;
                        collision.transform.parent = parentObj.transform;
                        //GunStateController.AddList(collision.gameObject.GetComponent<MovedObject>());
                        controll.AddChild(transform.GetComponent<MeshCollider>());
                        controll.AddChild(collision.transform.GetComponent<MeshCollider>());                        
                    }

                }

                checkContact = false;
                //GunStateController.AddList(this);
                ClearState();
                GrabGun.instance.CancelObj();

                //switch(LayerMask.LayerToName(collision.gameObject.layer))
                //{
                //    case "CatchObject":
                //        Debug.Log("케이스1");
                //        if(transform.gameObject.layer == LayerMask.NameToLayer("CatchObject"))
                //        {                     
                //            // { 충돌한 합쳐져 있는 오브젝트의 자식수만큼 옮기기
                //            GameObject[] careeObj = new GameObject[collision.transform.childCount];

                //            for (int j = 0; j < careeObj.Length; j++)
                //            {
                //                careeObj[j] = collision.transform.parent.GetChild(j).gameObject;
                //            }

                //            for (int j = 0; j < careeObj.Length; j++)
                //            {
                //                careeObj[j].transform.parent = transform;
                //                careeObj[j].GetComponent<MeshCollider>().convex = false;
                //            }
                //            // } 충돌한 합쳐져 있는 오브젝트의 자식수만큼 옮기기
                //        }
                //        else
                //        {                            
                //            transform.parent = collision.transform;
                //            myColid.convex = false;
                //        }
                //        Destroy(myRigid);
                //        GrabGun.instance.CancelObj();
                //        checkContact = false;
                //        break;
                //    case "Default":
                //        Debug.Log("케이스2");
                //        if (transform.gameObject.layer == LayerMask.NameToLayer("CatchObject"))
                //        {
                //            //Vector3 pos = transform.position - collision.contacts[i].point;
                //            //transform.position -= pos;
                //            Destroy(myRigid);
                //            for (int j = 0; j < childColid.Count; j++)
                //            {
                //                childColid[j].convex = false;
                //            }                            
                //        }
                //        else
                //        {
                //            parentObj = new GameObject();
                //            GunStateController.AddList(parentObj);
                //            parentObj.layer = LayerMask.NameToLayer("CatchObject");
                //            parentObj.transform.position = collision.contacts[i].point;
                //            transform.parent = parentObj.transform;
                //            Destroy(myRigid);
                //            myColid.convex = false;

                //        }
                //        GrabGun.instance.CancelObj();
                //        checkContact = false;
                //        break;
                //    case "MovedObject":
                //        Debug.Log("케이스3");

                //        // 충돌한 대상의 부모가 존재하고, 그 부모가 합치려고 만들어진 오브젝트라면
                //        if (collision.gameObject.transform.parent != null &&
                //            collision.transform.parent.gameObject.layer == LayerMask.NameToLayer("CatchObject"))
                //        {
                //            Debug.Log("11111");
                //            // { 닿은 오브젝트의 자식 수만큼 옮기기
                //            GameObject[] careeObj = new GameObject[collision.transform.childCount];

                //            for (int j = 0; j < careeObj.Length; j++)
                //            {
                //                careeObj[j] = collision.transform.parent.GetChild(j).gameObject;
                //            }

                //            for (int j = 0; j < careeObj.Length; j++)
                //            {
                //                careeObj[j].transform.parent = transform;
                //            }

                //            // } 닿은 오브젝트의 자식 수만큼 옮기기                                                                                  

                //            for(int j = 0; j < transform.childCount; j++)
                //            {
                //                transform.GetChild(j).GetComponent<MeshCollider>().convex = false;
                //                //careeObj[j].GetComponent<MeshCollider>().convex = false;
                //            }
                //        }

                //        // 나는 합쳐진 대상이고, 충돌한 오브젝트는 종속되지 않은 오브젝트라면
                //        else if(transform.gameObject.layer == LayerMask.NameToLayer("CatchObject") &&
                //            collision.transform.parent.gameObject.layer !=
                //            LayerMask.NameToLayer("CatchObject"))
                //        {
                //            Debug.Log("22222");
                //            collision.transform.parent = transform;
                //        }
                //        else
                //        {
                //            Debug.Log("33333");
                //            parentObj = new GameObject();
                //            GunStateController.AddList(parentObj);
                //            parentObj.layer = LayerMask.NameToLayer("CatchObject");
                //            parentObj.transform.position = collision.contacts[i].point;
                //            transform.parent = parentObj.transform;
                //            collision.transform.parent = parentObj.transform;

                //        }

                //        if (childColid.Count != 0)
                //        {
                //            for (int j = 0; j < childColid.Count; j++)
                //            {
                //                childColid[j].convex = false;
                //            }

                //        }
                //        else
                //        {
                //            myColid.convex = false;

                //        }

                //        Destroy(myRigid);
                //        GrabGun.instance.CancelObj();
                //        checkContact = false;
                //        break;
                //}
            }
        }
    }

    public void ChangedState()
    {
        myRigid = GetComponent<Rigidbody>();
        myRigid.mass = 1000f;
        myColid.material.dynamicFriction = 0f;
        myColid.material.bounciness = 0f;        
        Invoke("ChangedCheck", 1f);
    }
    
    public void CelarBond()
    {
        if (transform.parent != null)
        {
            myColid.convex = true;                          
            myRigid = transform.AddComponent<Rigidbody>();
            myRigid.mass = 1000f;
            myRigid.useGravity = true;
            myColid.material.dynamicFriction = 1f;
        }
    }
    void ChangedCheck()
    {
        checkContact = true;
    }

    void ClearState()
    {
        if(myRigid != null)
        {
            Destroy(myRigid);
        }

        myColid.convex = false;
    }

}
