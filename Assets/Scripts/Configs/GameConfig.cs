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
    public float ArmorDefault = 10;
    public float StatminaDefault = 100;
    public int ArrowDefault = 0;
    public float countDownNormalAttack = 0.8f;
    public float countDownSpinAttack = 0.2f;
    public float countDownBowAttack = 1;
}
