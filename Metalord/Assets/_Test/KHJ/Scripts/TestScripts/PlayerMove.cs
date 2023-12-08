using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Rigidbody myRb;
    private Vector3 moveVec;


    //npc 상호작용관련
    

    // Start is called before the first frame update
    void Start()
    {
        myRb = transform.GetComponent<Rigidbody>();
        moveSpeed = 10f;
        jumpForce = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        transform.position += moveVec * moveSpeed * Time.deltaTime;
        
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    myRb.velocity = Vector3.zero;
        //    myRb.AddForce(new Vector3(transform.position.x, transform.position.y * jumpForce, transform.position.z),ForceMode.Impulse);
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //myRb.velocity = Vector3.zero;
            Jump(jumpForce);
        }

        //if (playerInteract != null && Input.GetKeyDown(KeyCode.E))  
        //{
        //    Debug.Log("플레이어 E 키누르기 ");
        //    playerInteract.InteractNpc();
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        //IInteractableObject interObj =other.GetComponent<IInteractableObject>();
        //interObj?.Interact(); 
        IInteractNpc playerInteract = other.GetComponent<IInteractNpc>();
        
    }


    private void OnTriggerStay(Collider other)
    {
        //여기는 안에 있어야만 변화하는 사물의 메서드 작성

        IInteractNpc playerInteract = other.GetComponent<IInteractNpc>();
        if (Input.GetKeyDown(KeyCode.E))
        {
            //IInteractableObject interObj = other.GetComponent<IInteractableObject>();
            Debug.Log("이건 트리거 스테이");
            playerInteract.InteractNpc();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        IInteractableObject interObj = other.GetComponent<IInteractableObject>();
        interObj?.InteractOut();
        //TODO 여기에는 입장하기만 해도 변화하는 사물의 메서드 작성
    }

    public void Jump(float jumpF)
    {
        myRb.AddForce(Vector3.up * jumpF, ForceMode.Impulse);

    }
}
