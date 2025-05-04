using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : Singleton<MainMenuController>
{
    [Header("Main Menu")]
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button newGameBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button exitBtn;

    [Header("Select Mode Menu")]
    [SerializeField] private Button normalBtn;
    [SerializeField] private Button hardcoreBtn;
    [SerializeField] private Button backBtn;

    [Header("Component")]
    [SerializeField] private GameObject handleHomeMenu;
    [SerializeField] private GameObject handleMainMenuButtonGroup;
    [SerializeField] private GameObject handleSelectModeButtonGroup;
    [SerializeField] private GameObject evironmentMenu;
    [SerializeField] private Camera cameraEnvironmentMenu;




    // Start is called before the first frame update
    void Start()
    {
        continueBtn.onClick.AddListener(OnClickContinueBtn);
        newGameBtn.onClick.AddListener(OnClickNewGameBtn);
        settingBtn.onClick.AddListener(OnClickSettingBtn);
        exitBtn.onClick.AddListener(OnClickExitBtn);
        normalBtn.onClick.AddListener(OnClickNormalBtn);
        hardcoreBtn.onClick.AddListener(OnClickHardcoreBtn);
        backBtn.onClick.AddListener(OnClickBackBtn);
        if(GameManager.Instance.GameModel.HaveSave){
            continueBtn.gameObject.SetActive(true);
        }else{
            continueBtn.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickContinueBtn(){
        OnGoToGamePlay();
        if(LevelManager.Instance!=null){
            int sceneIndex = LevelManager.Instance.GetSceneIndex(GameManager.Instance.GameModel.CurrentLevel);
            if(sceneIndex>=0){
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
            }else{
                Debug.LogError("Not have scene index = "+sceneIndex);
            }
        }
    }
    public void OnClickNewGameBtn(){
        handleMainMenuButtonGroup.SetActive(false);
        handleSelectModeButtonGroup.SetActive(true);
    }
    public void OnClickSettingBtn(){
        PopupManager.Instance?.GetPopup("PopupSetting");
        
    }
    public void OnClickExitBtn(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnClickNormalBtn(){
        OnGoToGamePlay();
        GameManager.Instance.GameModel.ResetCharacterSaveData();
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        GameManager.Instance.GameModel.SetCurrentLevel(1);
        GameManager.Instance.GameModel.SetCurrentGameMode(0);
        GameManager.Instance.GameModel.SetSaveState(true);
        GameManager.Instance.GameModel.SaveData();
    }
    public void OnClickHardcoreBtn(){
        OnGoToGamePlay();
        GameManager.Instance.GameModel.ResetCharacterSaveData();
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        GameManager.Instance.GameModel.SetCurrentLevel(1);
        GameManager.Instance.GameModel.SetCurrentGameMode(1);
        GameManager.Instance.GameModel.SetSaveState(true);
        GameManager.Instance.GameModel.SaveData();
    }
    public void OnClickBackBtn(){
        handleMainMenuButtonGroup.SetActive(true);
        handleSelectModeButtonGroup.SetActive(false);
    }
    public void OnGoToGamePlay(){
        if(QuickLoadingController.Instance!=null){
            QuickLoadingController.Instance.ShowLoading();
        }
        handleMainMenuButtonGroup.SetActive(false);
        handleSelectModeButtonGroup.SetActive(false);
        handleHomeMenu.gameObject.SetActive(false);
        if(evironmentMenu!=null){
            evironmentMenu.SetActive(false);
        }
        if(evironmentMenu!=null){
            cameraEnvironmentMenu.enabled = false;
        }
    }
    public void OnBackToMenu(){
        if(QuickLoadingController.Instance!=null){
            QuickLoadingController.Instance.ShowLoading();
        }
        GameManager.Instance.GameModel.RollBackDataCheckPoint();
        RefreshMainMenu();
        handleMainMenuButtonGroup.SetActive(true);
        handleSelectModeButtonGroup.SetActive(false);
        handleHomeMenu.gameObject.SetActive(true);
        if(evironmentMenu!=null){
            evironmentMenu.SetActive(true);
        }
        if(evironmentMenu!=null){
            cameraEnvironmentMenu.enabled = true;
        }
    }
    public void RefreshMainMenu(){
        if(GameManager.Instance.GameModel.HaveSave){
            continueBtn.gameObject.SetActive(true);
        }else{
            continueBtn.gameObject.SetActive(false);
        }
    }
}
