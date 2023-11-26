using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody myRb;
    
    // Start is called before the first frame update
    void Start()
    {
        myRb = transform.GetComponent<Rigidbody>();
        moveSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(hAxis, 0, vAxis).normalized;
        myRb.velocity = inputDir * moveSpeed;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractableObject interObj =other.GetComponent<IInteractableObject>();
        interObj?.Interact();
    }
}
