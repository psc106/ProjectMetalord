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
    public bool isInteract = false;
    IInteractNpc playerInteract = null;
    public bool isMove = true;
    // Start is called before the first frame update
    void Start()
    {
        isMove = true;
        isInteract = false;

        myRb = transform.GetComponent<Rigidbody>();
        moveSpeed = 10f;
        jumpForce = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            MoveCharacter();
            Jump(jumpForce);
        }
        if(isInteract)
        {
            PushE();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //IInteractableObject interObj =other.GetComponent<IInteractableObject>();
        //interObj?.Interact(); 
        if(other.CompareTag("Finish"))
        {
            playerInteract = other.GetComponent<IInteractNpc>();
            isInteract = true;
        }
        
    }

    private void OnTriggerExit(Collider other) 
    {
        //IInteractableObject interObj = other.GetComponent<IInteractableObject>();
        //interObj?.InteractOut();
        if (other.CompareTag("Finish"))
        {
            playerInteract = null;
            isInteract = false;
        }
    }

    public void MoveCharacter()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        transform.position += moveVec * moveSpeed * Time.deltaTime;
    }
    public void Jump(float jumpF)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRb.AddForce(Vector3.up * jumpF, ForceMode.Impulse);
        }
    }

    public void PushE()
    {
        if (playerInteract != null && Input.GetKeyDown(KeyCode.E))
        {
            isMove = false;
            isInteract = false;
            Debug.Log("플레이어 E 키누르기 ");
            playerInteract.InteractNpc();
        }
    }
}
