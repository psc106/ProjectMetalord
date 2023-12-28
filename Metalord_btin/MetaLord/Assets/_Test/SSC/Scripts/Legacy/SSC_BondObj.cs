using Unity.VisualScripting;
using UnityEngine;

public class SSC_BondObj : MonoBehaviour
{    
    MeshCollider myColider;        
    public LayerMask layerMask;
    public bool isGrabed;

    [HideInInspector] public Rigidbody myRigid;

    private void Awake()
    {            
        myColider = GetComponent<MeshCollider>();

        layerMask = 1 << LayerMask.NameToLayer("Default") |
            1 << LayerMask.NameToLayer("NPC") |
            1 << LayerMask.NameToLayer("StaticObject") |
            1 << LayerMask.NameToLayer("MovedObject");
    }

    private void OnCollisionStay(Collision collision)
    {
        // 이미 본드 동작을 하는 오브젝트를 다시 그랩하면 그랩하는순간 충돌면을 체크하여 그랩 해제됨에 따라 상태를 제어할 bool값 추가
        if(isGrabed == true)
        {
            return;
        }

        // 충돌이 일어나는 지점을 모두 체크
        for(int i = 0; i < collision.contactCount; i++)
        {
            Vector3 dir = collision.contacts[i].normal;

            Ray ray = new Ray(collision.contacts[i].point + dir, -dir);

            if (PaintTarget.RayChannel(ray, 1.5f, layerMask) == 0 && collision.gameObject.GetComponent<Controller_Physics>() == null)
            {
                if(collision.transform.GetComponent<NpcBase>() != null)
                {
                    collision.transform.GetComponent<NpcBase>().ChangedState(npcState.objectAttached);
                    GunStateController.AddList(collision.transform.GetComponent<NpcBase>());
                }

                transform.parent = null;
                GameObject parentObj = new GameObject();
                parentObj.transform.position = transform.position;
                transform.parent = parentObj.transform;
                collision.transform.parent = parentObj.transform;
                //GunStateController.AddList(this);

                if (transform.GetComponent<Rigidbody>() != null)
                {
                    Destroy(transform.GetComponent<Rigidbody>());
                    myColider.convex = false;
                    GrabGun.instance.CancelObj();

                    //충돌이 일어난 오브젝트의 재 충돌감지 진입을 막기위해 값 변경
                    isGrabed = true;
                }
            }            
        }
    }

    void Update()
    {
        if(myRigid)
        {
            if(myRigid.IsSleeping())
            {
                Destroy(myRigid);
                myColider.convex = false;
                myRigid = null;
            }
        }
    }

    public void CelarBond()
    {
        if(transform.parent != null)
        {
            transform.parent = null;

            myColider.convex = true;
            myRigid = transform.AddComponent<Rigidbody>();
            myRigid.mass = 1000f;
            myRigid.useGravity = true;            
        }
    }

    // 그랩 시도시 지정된 시간 이후부터 충돌감지를 하게할 메소드
    // GrabGun에서 인보크 메소드로 호출한다.
    public void StateChagned()
    {        
        isGrabed = false;
    }
}
