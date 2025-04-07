using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
    // Start is called before the first frame update
    // public CharacterStatsConfig _baseStats;
    // private CharacterStats _currentStats;
    private GameConfig _gameConfig;
    private bool _playerIsDie;
    private Animator _animator;
    public bool PlayerIsDie => _playerIsDie;
    public Action OnPlayerDie;
    void Awake()
    {
    }
    void Start()
    {
        _gameConfig = GameConfig.Load();
        GameManager.Instance.SetPlayerControler(this);
        _animator = gameObject.GetComponent<Animator>();
        PlayerHealthBar.Instance.SetupHealthBar(GameManager.Instance.CurrentHealth,_gameConfig.HealthDefault);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDame(float dame){
        GameManager.Instance.GameModel.SetHealth(Mathf.Clamp(GameManager.Instance.CurrentHealth-(dame-(dame*(GameManager.Instance.CurrentArmor/100))),0f,_gameConfig.HealthDefault));
        PlayerHealthBar.Instance.OnHealthChange(GameManager.Instance.CurrentHealth);
        Debug.Log("Player take dame: "+ dame+" Current Health: "+GameManager.Instance.CurrentHealth);
        if(GameManager.Instance.CurrentHealth>0&&dame>0){
            _animator.SetTrigger("TakeDame");
        }
        if(GameManager.Instance.CurrentHealth==0){
            Debug.Log("Player Die");
            _playerIsDie = true;
            _animator.SetTrigger("Die");
            OnPlayerDie?.Invoke();
        }
    }
}
