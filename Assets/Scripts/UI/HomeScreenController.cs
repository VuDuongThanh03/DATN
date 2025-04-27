using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickContinueBtn(){
        handleMainMenuButtonGroup.SetActive(false);
        handleSelectModeButtonGroup.SetActive(false);
        handleHomeMenu.SetActive(false);
        if(evironmentMenu!=null){
            evironmentMenu.SetActive(false);
        }
        if(evironmentMenu!=null){
            cameraEnvironmentMenu.enabled = false;
        }
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
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
        handleMainMenuButtonGroup.SetActive(false);
        handleSelectModeButtonGroup.SetActive(false);
        handleHomeMenu.gameObject.SetActive(false);
        if(evironmentMenu!=null){
            evironmentMenu.SetActive(false);
        }
        if(evironmentMenu!=null){
            cameraEnvironmentMenu.enabled = false;
        }
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
    public void OnClickHardcoreBtn(){
        handleMainMenuButtonGroup.SetActive(false);
        handleSelectModeButtonGroup.SetActive(false);
        handleHomeMenu.gameObject.SetActive(false);
        if(evironmentMenu!=null){
            evironmentMenu.SetActive(false);
        }
        if(evironmentMenu!=null){
            cameraEnvironmentMenu.enabled = false;
        }
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
    public void OnClickBackBtn(){
        handleMainMenuButtonGroup.SetActive(true);
        handleSelectModeButtonGroup.SetActive(false);
    }
}
