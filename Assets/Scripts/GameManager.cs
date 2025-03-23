using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static PlayerController Player;
    private float _ratioRotateSpeed = 2f;
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
}
