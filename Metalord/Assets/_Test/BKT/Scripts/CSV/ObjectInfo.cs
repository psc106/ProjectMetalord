using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 도감 오브젝트의 정보_CSV를 통해 반영
/// 231130_배경택
/// </summary>
public class ObjectInfo : MonoBehaviour
{
    public int id { get; set; }
    public string id_Description { get; set; }
    public string item_Name { get; set; }
    public int zone { get; set; } // 1 = 주방, 2= 거실, 3= 아기방
    public bool obtained { get; set; }
    public string description { get; set; }

    
}
