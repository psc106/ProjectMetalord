using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        1 << LayerMask.NameToLayer("GrabedObject");
    }

    public void InitParentMovedObject()
    {        
        myRigid = GetComponent<Rigidbody>();
        myRigid.mass = 1000f;

        layerMask = 1 << LayerMask.NameToLayer("Default") |
        1 << LayerMask.NameToLayer("NPC") |
        1 << LayerMask.NameToLayer("StaticObject") |
        1 << LayerMask.NameToLayer("MovedObject") |
        1 << LayerMask.NameToLayer("GrabedObject");
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

    //// 그랩한 물건이 이동형 오브젝트와 부딪힐때마다 물리력 행사 콜백

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MovedObject"))
        {
            if (collision.gameObject.GetComponent<PaintTarget>().CheckPainted())
            {
                return;
            }

            if (collision.gameObject.GetComponent<OverapObject>() == null)
            {
                collision.gameObject.AddComponent<OverapObject>().InitOverap();
            }
        }
    }

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
            Ray reflectRay = new Ray(collision.contacts[i].point, dir);
            Debug.DrawLine(collision.contacts[i].point, dir, Color.red);

            if (PaintTarget.RayChannel(ray, 1.5f, layerMask) == 0 && collision.gameObject.GetComponent<Controller_Physics>() == null && checkContact)
            {
                GameObject parentObj = null;    
                
                // 충돌한 오브젝트의 부모가 없을경우
                if(collision.transform.parent == null)
                {                    
                    // 상위 오브젝트 생성
                    parentObj = new GameObject();
                    parentObj.transform.gameObject.layer = LayerMask.NameToLayer("GrabedObject");                    
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

                    // TODO : 레이어 체크가 아닌 CatchObject 스크립트로 여부 확인
                    // 부모가 상위 오브젝트 경우
                    if(contactObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("GrabedObject"))
                    {                                        
                        // HashSet 갱신    
                        transform.parent = contactObj.transform.parent;
                        contactObj.transform.parent.GetComponent<CatchObject>().AddChild(transform.GetComponent<MeshCollider>());        

                    }
                    // 부모가 고정형 오브젝트 경우
                    else if(contactObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("Default"))
                    {
                        // 부딪힌 오브젝트도 고정형이면
                        if(contactObj.transform.gameObject.layer == LayerMask.NameToLayer("Default"))
                        {
                            // 클래스 여부로 기존 고정형 오브젝트인지 합쳐진 오브젝트인지
                            if(contactObj.transform.parent.GetComponent<CatchObject>())
                            {
                                transform.SetParent(contactObj.transform.parent);
                                //if(contactObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("Default"))
                                //{                                    
                                //    if (contactObj.transform.GetComponent<CatchObject>())
                                //    {
                                //        Debug.Log("222");
                                //        //for(int k = transform.childCount-1; k >= 0; k--)
                                //        //{
                                //        //    transform.GetChild(k).SetParent( contactObj.transform);
                                //        //}
                                //        ////자식 옮기기
                                //    }
                                //    else
                                //    {
                                //        Debug.Log("333");
                                //        transform.SetParent(contactObj.transform.parent);
                                //    }
                                //}
                                //else if(contactObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("GrabedObject"))
                                //{
                                //    if (transform.parent.GetComponent<CatchObject>())
                                //    {
                                //        for (int k = transform.childCount - 1; k >= 0; k--)
                                //        {
                                //            transform.GetChild(k).SetParent(contactObj.transform.parent);
                                //        }
                                //        //자식 옮기기
                                //    }
                                //    else
                                //    {
                                //        transform.SetParent(contactObj.transform.parent);
                                //        //자신 옮기기
                                //    }
                                //}
                            }
                            else 
                            {
                                // 상위 오브젝트 생성
                                parentObj = new GameObject();
                                parentObj.transform.gameObject.layer = LayerMask.NameToLayer("Default");
                                CatchObject controll = parentObj.AddComponent<CatchObject>();
                                GunStateController.AddList(controll);
                                parentObj.transform.position = collision.contacts[i].point;

                                // 그랩한 오브젝트 상위 고정형 오브젝트 종속, HashSet 갱신    
                                transform.parent = parentObj.transform; 
                                controll.AddChild(transform.GetComponent<MeshCollider>());

                            }
                        }
                        else
                        {
                            //Debug.Log("여기");
                            // 충돌한 이동형 오브젝트가 고정형에 집합된 형태라면 
                            if(contactObj.transform.parent.GetComponent<CatchObject>() != null &&
                                contactObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("Default"))
                            {
                                //Debug.Log("===");
                                transform.parent = contactObj.transform.parent;
                                checkContact = false;                                
                                ClearState();
                                GrabGun.instance.CancelObj();
                                return;
                            }
                            // 상위 오브젝트 생성
                            parentObj = new GameObject();
                            parentObj.transform.gameObject.layer = LayerMask.NameToLayer("GrabedObject");                        
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

                }

                checkContact = false;
                //GunStateController.AddList(this);
                ClearState();
                GrabGun.instance.CancelObj();
                
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
