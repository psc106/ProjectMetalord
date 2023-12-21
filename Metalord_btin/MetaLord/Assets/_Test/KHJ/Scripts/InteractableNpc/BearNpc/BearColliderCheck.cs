using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearColliderCheck : MonoBehaviour
{
    public SphereCollider interactCollider;
    public LayerMask waterLayer;
    public BearInteractUI interactUI;
    bool isAim = false;
    // Start is called before the first frame update

    void Start()
    {
        interactCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHeadCollider();
    }

    public void CheckHeadCollider()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, waterLayer)) //곰으로 변경 예정
        {
            Debug.Log("head check 들어오나?");
        }
    }
}
