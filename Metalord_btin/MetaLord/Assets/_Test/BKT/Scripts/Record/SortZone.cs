using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortZone : MonoBehaviour
{
    [SerializeField] private Zone zone;

    public void ClickButton()
    {
        RecordManager.instance.SortZone((int)zone);
    }
}
