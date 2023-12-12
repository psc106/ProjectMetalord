using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어 이동 속도
    public float jumpForce = 5f; // 플레이어 점프 힘
    public bool isUmbrella = false; // 플레이어가 우산을 펼쳤는지
    public float INTERECT_RADIOUS = 1f; // 상호작용 반경

    private bool toggleUmbrella = false;
    private Rigidbody rb; // Rigidbody 컴포넌트

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
    }

    void Update()
    {
        // 움직임 처리
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection);

        // 점프 처리
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // 우산 처리
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(isUmbrella == false) isUmbrella = true;
            else isUmbrella = false;

            Debug.Log(isUmbrella);
        }

        // NPC와 상호작용
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckNPC();
        }
    }

    /// <summary>
    /// [임시] 플레이어가 NPC와 상호작용할때 사용하는 함수
    /// </summary>
    void CheckNPC()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position,INTERECT_RADIOUS); // 반경 유닛 내의 충돌체 검출

        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("NPC"))
            {
                Debug.Log("NPC 상호작용 함수 호출");

                // 상대방 객체의 함수 호출
                col.GetComponent<NPC>().InterectNPC();
                break; // NPC를 찾아서 함수 호출했다면 foreach문 종료
            }
        }
    }

    // 점프 함수
    void Jump()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.01f) // 플레이어가 바닥에 있을 때만 점프 가능하도록
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }


}