using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static PlayerController Player;
    private float _ratioRotateSpeed = 0.3f;
    public float RatioRotateSpeed => _ratioRotateSpeed;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)){
            PopupManager.Instance.GetPopup("PopupExample");
        }
    }
}
