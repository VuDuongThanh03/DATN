using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "config/TradeConfig")]
public class TradeConfig : ScriptableObject
{
    static TradeConfig cache;

    public static TradeConfig Load()
    {
        return cache ??= Resources.Load<TradeConfig>("Configs/TradeConfig");
    }
    public int HealthBottlePrice = 5;
    public int StaminaBottlePrice = 5;
    public int ArrowPrice = 3;
    public int UpdateLevel1Price = 30;
    public int UpdateLevel2Price = 50;
}
