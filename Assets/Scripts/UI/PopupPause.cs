using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupPause : PopupBase
{
    // Start is called before the first frame update
    [SerializeField] Button continueButton;
    [SerializeField] Button settingButton;
    [SerializeField] Button backToMenuButton;


    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    public void Init()
    {
        
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnBackBtnClick);
        settingButton.onClick.RemoveAllListeners();
        settingButton.onClick.AddListener(OnClickSettingButton);
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
    private void OnClickSettingButton(){
        if(PopupManager.Instance!=null){
            PopupManager.Instance.GetPopup("PopupSetting");
        }
    }
    private void OnClickBackToMenuButton(){
        SceneManager.UnloadSceneAsync(1);
        if(MainMenuController.Instance!=null){
            MainMenuController.Instance.OnBackToMenu();
            OnBackBtnClick();
        }
    }
}
