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
        GameManager.Player = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDame(float dame){
        _currentStats.health=Mathf.Clamp(_currentStats.health-(dame-(dame*(_currentStats.armor/100))),0f,_baseStats.CharacterStats.health);
        Debug.Log("Player take dame: "+ dame+" Current Health: "+_currentStats.health);
        if(_currentStats.health==0){
            Debug.Log("Player Die");
            // gameObject.SetActive(false);
        }
    }
}
