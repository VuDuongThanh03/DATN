using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupEvent : PopupBase
{
    // Start is called before the first frame update
    [SerializeField] Button backToMenuButton;


    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    public void Init()
    {
        backToMenuButton.onClick.RemoveAllListeners();
        backToMenuButton.onClick.AddListener(OnClickBackToMenuButton);
        
#if UNITY_EDITOR
        // Cursor.lockState = CursorLockMode.None;
#endif
    }

    public override void OnBackBtnClick()
    {
        base.OnBackBtnClick();
#if UNITY_EDITOR
        // Cursor.lockState = CursorLockMode.Locked;
#endif
    }
    private void OnClickBackToMenuButton(){
        OnBackBtnClick();
    }
}
