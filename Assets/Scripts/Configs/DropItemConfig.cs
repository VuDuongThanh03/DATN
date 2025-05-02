using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DropItemConfig", menuName = "config/DropItemConfig")]
public class DropItemConfig : ScriptableObject
{
    static DropItemConfig cache;

    public static DropItemConfig Load()
    {
        return cache ??= Resources.Load<DropItemConfig>("Configs/DropItemConfig");
    }
    public List<DropItemData> dropItemDatas;
}
[Serializable]
public class DropItemData{
    public GameObject prefabDropItem;
    public int minQuantity;
    public int maxQuantity;
    public int weight;
}