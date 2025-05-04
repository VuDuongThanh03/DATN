using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupLose : PopupBase
{
    // Start is called before the first frame update
    [SerializeField] Button playAgainButton;
    [SerializeField] Button backToMenuButton;


    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    public void Init()
    {
        playAgainButton.onClick.RemoveAllListeners();
        playAgainButton.onClick.AddListener(OnClickPlayAgainButton);
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
    private void OnClickPlayAgainButton(){
        LevelManager.Instance.PlayAgain(()=>{OnBackBtnClick();});
    }
    private void OnClickBackToMenuButton(){
        int currentSceneIndex = LevelManager.Instance.GetSceneIndex(GameManager.Instance.GameModel.CurrentLevel);
        if(currentSceneIndex>=0){
            SceneManager.UnloadSceneAsync(currentSceneIndex);
            if(MainMenuController.Instance!=null){
                MainMenuController.Instance.OnBackToMenu();
                OnBackBtnClick();
            }
        }
    }
}
