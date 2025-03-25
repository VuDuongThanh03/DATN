using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private AttackController _attackController;
    private float _ratioRotateSpeed = 2f;
    public PlayerController PlayerController => _playerController;
    public AttackController AttackController => _attackController;
    public float RatioRotateSpeed => _ratioRotateSpeed;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)){
            PopupManager.Instance.GetPopup("PopupExample");
        }
    }
    public void SetRotateSpeedBowAttack(bool isStart){
        if(isStart){
            _ratioRotateSpeed = 6f;
        }else{
            _ratioRotateSpeed = 2f;
        }
    }
    public void SetPlayerControler(PlayerController playerController){
        _playerController = playerController;
    }
    public void SetAttackControler(AttackController attackController){
        _attackController = attackController;
    }
}
