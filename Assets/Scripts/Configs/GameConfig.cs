using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "config/GameConfig")]
public class GameConfig : ScriptableObject
{
    static GameConfig cache;

    public static GameConfig Load()
    {
        return cache ??= Resources.Load<GameConfig>("Configs/GameConfig");
    }
    public int ConfigVersion;
    public int CoinDefault;
    public float HealthDefault = 100;
    public int MaxCountItemHealth = 5;
    public int ReturnHealthValueUseItem = 50;
    public float ArmorDefault = 10;
    public float StatminaDefault = 100;
    public int MaxCountItemStamina = 5;
    public float ReturnStaminaPerSecond = 2;
    public int ReturnStaminaValueUseItem = 50;
    public float SwordAttackCost = 5;
    public float BowAttackCost = 10;
    public float AngryEnergyDefault = 0;
    public float MaxAngryEnergy = 100;
    public int ArrowDefault = 0;
    public int MaxArrow = 0;
    public float countDownNormalAttack = 0.8f;
    public float countDownSpinAttack = 0.2f;
    public float countDownBowAttack = 1;
}
