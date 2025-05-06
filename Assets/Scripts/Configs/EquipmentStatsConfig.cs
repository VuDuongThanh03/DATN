using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EquipmentStatsConfig", menuName = "Stat/EquimentStats")]
public class EquipmentStatsConfig : ScriptableObject
{
    static EquipmentStatsConfig cache;

    public static EquipmentStatsConfig Load()
    {
        return cache ??= Resources.Load<EquipmentStatsConfig>("Configs/EquipmentStatsConfig");
    }
    public List<EquipmentStat> equipmentStats;
    public EquipmentStat GetEquipmentStat(int level){
        foreach (var item in equipmentStats)
        {
            if(item.level==level){
                return item;
            }
        }
        return null;
    }

}
[Serializable]
public class EquipmentStat{
    public int level;
    public float armor;
    public float swordDamage;
    public float bowDamage;

}
