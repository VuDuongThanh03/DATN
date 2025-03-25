using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
    // Start is called before the first frame update
    public CharacterStatsConfig _baseStats;
    private CharacterStats _currentStats;
    private bool _playerIsDie;
    private Animator _animator;
    public bool PlayerIsDie => _playerIsDie;
    public Action OnPlayerDie;
    void Awake()
    {
    }
    void Start()
    {
        GameManager.Instance.SetPlayerControler(this);
        _animator = gameObject.GetComponent<Animator>();
        _currentStats = new CharacterStats();
        _currentStats.health = _baseStats.CharacterStats.health;
        _currentStats.armor = _baseStats.CharacterStats.armor;
        _currentStats.health = _baseStats.CharacterStats.health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDame(float dame){
        _currentStats.health=Mathf.Clamp(_currentStats.health-(dame-(dame*(_currentStats.armor/100))),0f,_baseStats.CharacterStats.health);
        Debug.Log("Player take dame: "+ dame+" Current Health: "+_currentStats.health);
        if(_currentStats.health>0&&dame>0){
            _animator.SetTrigger("TakeDame");
        }
        if(_currentStats.health==0){
            Debug.Log("Player Die");
            _playerIsDie = true;
            _animator.SetTrigger("Die");
            OnPlayerDie?.Invoke();
        }
    }
}
