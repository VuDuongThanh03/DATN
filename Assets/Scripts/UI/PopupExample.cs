using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupExample : PopupBase
{
    // Start is called before the first frame update
    [SerializeField] Button closeButton;
    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    public void Init()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(OnBackBtnClick);
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
#endif
    }

    public override void OnBackBtnClick()
    {
        base.OnBackBtnClick();
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }
}
