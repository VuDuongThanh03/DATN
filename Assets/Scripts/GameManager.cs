using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static PlayerController Player;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)){
            PopupManager.Instance.GetPopup("PopupExample");
        }
    }
}
