using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVent : MonoBehaviour
{
    public float upwardForce = 15f; // 위로 가해질 힘

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // 트리거에 들어와있는 오브젝트가 Player이고
        {
            if (other.GetComponent<Player>().isUmbrella == true) // 플레이어가 우산을 펼치면
            {
                Rigidbody rb = other.GetComponent<Rigidbody>(); // 플레이어의 Rigidbody 가져오기

                if (rb != null)
                {
                    // Rigidbody에 위쪽으로의 힘을 가하면서 이동시키기
                    rb.AddForce(Vector3.up * upwardForce * Time.deltaTime, ForceMode.Impulse);
                }
            }
        }
    }
}
