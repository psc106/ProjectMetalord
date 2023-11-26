using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Rigidbody myRb;
    private Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        myRb = transform.GetComponent<Rigidbody>();
        moveSpeed = 10f;
        jumpForce = 15f;
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
            myRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractableObject interObj =other.GetComponent<IInteractableObject>();
        interObj?.Interact();
    }
}
