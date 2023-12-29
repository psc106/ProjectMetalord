using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchObject : MonoBehaviour
{
    Rigidbody myRigid;
    HashSet<MeshCollider> childColid = new HashSet<MeshCollider>();
    bool checkContact;
    LayerMask layerMask;

    private void Awake()
    {
        checkContact = false;        
        layerMask = 1 << LayerMask.NameToLayer("Default") |
        1 << LayerMask.NameToLayer("NPC") |
        1 << LayerMask.NameToLayer("StaticObject") |
        1 << LayerMask.NameToLayer("MovedObject") |
        1 << LayerMask.NameToLayer("GrabedObject");
    }

    public void AddChild(MeshCollider _child)
    {
        childColid.Add(_child);        
    }

    public void SetUpMesh()
    {
        foreach (MeshCollider col in childColid)
        {
            col.convex = true;
        }
    }

    private void Update()
    {
        if(myRigid)
        {
            if(myRigid.IsSleeping())
            {
                Destroy(myRigid);

                foreach(MeshCollider col in childColid)
                {
                    col.convex = false;
                }                
            }
        }
    }

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
                if (collision.transform.parent == null)
                {                                        
                    // 충돌한 오브젝트를 내 하위에 종속
                    collision.transform.parent = transform;     
                    
                    // 자식 메쉬콜라이더 갱신
                    AddChild(collision.transform.GetComponent<MeshCollider>());
                }
                // 부모가 존재할 경우
                else
                {
                    // 지정 오브젝트는 충돌한 오브젝트의 부모
                    GameObject contactObj = collision.transform.parent.gameObject;

                    // 부모가 상위 오브젝트 경우
                    if (contactObj.gameObject.layer == LayerMask.NameToLayer("GrabedObject"))
                    {                                                                    
                        CareeToContact(contactObj);
                    }
                    // 부모가 고정형 오브젝트 경우
                    else if (contactObj.gameObject.layer == LayerMask.NameToLayer("Default"))
                    {
                        // TODO : 스태틱 오브젝트와 어떤 연계를 할 것인지
                    }

                }

                checkContact = false;                                
                GrabGun.instance.CancelObj();
            }
        }
    }

    public void ChangedState()
    {
        myRigid = GetComponent<Rigidbody>();
        myRigid.mass = 1000f;
        Invoke("ChangedCheck", 1f);
    }

    void ChangedCheck()
    {
        checkContact = true;
    }

    void CareeToContact(GameObject contactObj)
    {
        GameObject[] myChild = new GameObject[transform.childCount];

        // 내 자식만큼 오브젝트 캐싱
        for (int i = 0; i < myChild.Length; i++)
        {
            myChild[i] = transform.GetChild(i).gameObject;
        }

        // 오브젝트 부모 변경 및 해쉬 갱신
        for (int i = 0; i < myChild.Length; i++)
        {
            myChild[i].transform.parent = contactObj.transform;
            myChild[i].GetComponent<MeshCollider>().convex = false;

            // 충돌한 오브젝트의 상위 오브젝트 해쉬 갱신
            contactObj.transform.GetComponent<CatchObject>().AddChild(myChild[i].transform.GetComponent<MeshCollider>());            
        }

        // 컨트롤러에 담겨있는 Hash중 나 자신을 제거 후 오브젝트채로 파괴
        GunStateController.catchList.Remove(this);
        Destroy(this.gameObject);
    }

}
