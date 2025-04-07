using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyStats", menuName = "Stat/EnemyStats")]
public class EnemyStatsConfig : ScriptableObject
{
    public EnemyStats EnemyStats;
}
[System.Serializable]
public class EnemyStats
{
    // Start is called before the first frame update
    public float health;
    public float armor;
}
