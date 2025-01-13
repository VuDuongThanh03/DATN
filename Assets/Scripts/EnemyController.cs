using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class EnemyController : MonoBehaviour,IDamageable
{
    public EnemyStatsConfig _baseStats;
    private EnemyStats _currentStats;

    // Start is called before the first frame update
    void Start()
    {
        _currentStats = new EnemyStats();
        _currentStats.health = _baseStats.EnemyStats.health;
        _currentStats.armor = _baseStats.EnemyStats.armor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDame(float dame)
    {
        _currentStats.health=Mathf.Clamp(_currentStats.health-(dame-(dame*(_currentStats.armor/100))),0f,_baseStats.EnemyStats.health);
        Debug.Log("Take dame: "+ dame+" Current Health: "+_currentStats.health);
        if(_currentStats.health==0){
            Debug.Log("Enemy Die");
            gameObject.SetActive(false);
        }
    }
}
