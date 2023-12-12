using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int width = 1;
    public int height = 1;
    public int psc = 0;

    public Sprite itemIcon;

    [field: SerializeField] public string Id { get; set; }
}
