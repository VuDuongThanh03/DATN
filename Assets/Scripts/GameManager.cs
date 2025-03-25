using System.Collections;
using System.Collections.Generic;
using DATN;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private PlayerController _playerController;
    private AttackController _attackController;
    private PlayerMovementController _playerMovementController;
    private float _ratioRotateSpeed = 2f;
    public PlayerController PlayerController => _playerController;
    public AttackController AttackController => _attackController;
    public PlayerMovementController PlayerMovementController => _playerMovementController;
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
    public void SetPlayerMovementController(PlayerMovementController playerMovementController){
        _playerMovementController = playerMovementController;
    }
}
