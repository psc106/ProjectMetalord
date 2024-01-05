using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OverapObject : MonoBehaviour
{
    //Rigidbody myRigid;
    //MeshCollider myColid;
    //OverapObject myScript;
    //MovedObject myMoved;

    //float ySpeed = default;
    //float decrement = 0.8f;
    //int reflectCount = 0;
    //int reflectLimit = 20;
    //int checkCount = 0;

    //public void InitOverap(GunStateController _state)
    //{
    //    if (myScript == null)
    //    {
    //        myScript = this;
    //        myScript = GetComponent<OverapObject>();
    //        myMoved  = GetComponent<MovedObject>();
    //    }

    //    if(myRigid == null)
    //    {
    //        transform.gameObject.AddComponent<Rigidbody>();
    //        myRigid = transform.GetComponent<Rigidbody>();
    //        myRigid.mass = 10f;
    //        myRigid.AddForce(Vector3.down * 100f, ForceMode.Force);
    //    }

    //    if(myColid == null)
    //    {
    //        myColid = GetComponent<MeshCollider>();
    //        myColid.material.dynamicFriction = 1f;
    //        myColid.convex = true;
    //    }

    //    ySpeed = 0f;
    //    myColid.material.dynamicFriction = 0f;
    //    myColid.material.bounciness = 0.8f;
    //    myMoved.CareeState(_state, myRigid);             
    //}

    //private void Update()
    //{
    //    if(myScript)
    //    {            
    //        if(reflectCount >= reflectLimit && myRigid.velocity.magnitude <= 0.2f)
    //        {
    //            Debug.Log("슬립");
    //            Destroy(myRigid);
    //            myRigid = null;
    //            myColid.convex = false;
    //            Destroy(myScript);
    //            return;
    //        }

    //        ySpeed -= Time.deltaTime * 20f;

    //        Vector3 downSpeed = myRigid.velocity;
    //        downSpeed.y += ySpeed;

    //        myRigid.velocity = downSpeed;
    //        //Debug.Log("매그니튜드 : "+myRigid.velocity.magnitude);
    //    }
        
    //}

    //private void OnCollisionEnter(Collision collision)
    //{        
    //    //for(int i = 0; i < collision.contactCount; i++)
    //    //{
    //    //    if(collision.contacts[i].point.normalized.y <= 0.1f)
    //    //    {
    //    //        //Debug.Log(collision.contacts[i].point.normalized.y);
    //    //        Vector3 tempVelcotity = new Vector3(myRigid.velocity.x * decrement, -(ySpeed * decrement * 3), myRigid.velocity.z * decrement);
    //    //        ySpeed = 0f;
    //    //        myRigid.velocity = tempVelcotity;
    //    //        reflectCount++;                
    //    //        break;
    //    //    }
    //    //}

    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    for (int i = 0; i < collision.contactCount; i++)
    //    {            
    //        if (collision.contacts[i].point.normalized.y <= 0.1f)
    //        {
    //            checkCount++;      
                
    //            if(checkCount > 5)
    //            {                   
    //                reflectCount++;
    //                checkCount = 0;
    //                Vector3 tempVelcotity = new Vector3(myRigid.velocity.x * decrement, -(ySpeed * decrement), myRigid.velocity.z * decrement);
    //                myRigid.velocity = tempVelcotity;                    
    //                ySpeed = 0f;
    //                break;

    //            }
    //        }
    //    }
    //}
}
