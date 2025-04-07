using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{
    Coin,
    HPBottle,
    Arrow,

}
public class CollectableItem : MonoBehaviour,ICollectable
{
    [SerializeField] ItemType itemType;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
