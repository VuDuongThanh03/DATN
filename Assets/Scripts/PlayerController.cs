using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
    // Start is called before the first frame update
    public CharacterStatsConfig _baseStats;
    private CharacterStats _currentStats;

    void Start()
    {
        _currentStats = _baseStats.CharacterStats;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDame(float dame){
        
    }
}
