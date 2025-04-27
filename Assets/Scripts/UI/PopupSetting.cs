using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : PopupBase
{
    // Start is called before the first frame update
    [SerializeField] Button closeButton;
    [SerializeField] Slider rotateSpeedSlider;
    [SerializeField] TMP_Dropdown graphicsQuality;
    [SerializeField] Toggle soundToggle;
    [SerializeField] Toggle musicToggle;
    [SerializeField] List<string> options;

    float rotateSpeed = 0.5f;
    int graphicsQualityIndex = 2;
    int isSound = 1;
    int isMusic = 1;

    private const float MIN_ROTATE_SPEED = 3;
    private const float MAX_ROTATE_SPEED = 15;


    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    public void Init()
    {
        options = new List<string>{"Low","Medium","High"};
        graphicsQuality.options.Clear();
        graphicsQuality.AddOptions(options);
        if(PlayerPrefs.HasKey("RotateSpeed")){
            rotateSpeed = PlayerPrefs.GetFloat("RotateSpeed");
        }
        if(PlayerPrefs.HasKey("GraphicQuality")){
            graphicsQualityIndex = PlayerPrefs.GetInt("GraphicQuality");
        }
        if(PlayerPrefs.HasKey("IsSound")){
            isSound = PlayerPrefs.GetInt("IsSound");
        }
        if(PlayerPrefs.HasKey("IsMusic")){
            isMusic = PlayerPrefs.GetInt("IsMusic");
        }
        rotateSpeedSlider.value = rotateSpeed;
        graphicsQuality.value = graphicsQualityIndex;
        graphicsQuality.RefreshShownValue();
        soundToggle.isOn = isSound==1;
        musicToggle.isOn = isMusic==1;

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(OnBackBtnClick);
        rotateSpeedSlider.onValueChanged.AddListener(OnRotateSpeedChange);
        graphicsQuality.onValueChanged.AddListener(OnGraphicQualityChange);
        soundToggle.onValueChanged.AddListener(OnSoundToggleChange);
        musicToggle.onValueChanged.AddListener(OnMusicToggleChange);
#if UNITY_EDITOR
        // Cursor.lockState = CursorLockMode.None;
#endif
    }
    private void OnRotateSpeedChange(float value){

    }
    private void OnGraphicQualityChange(int value){
        if(value==0){
            QualitySettings.SetQualityLevel(0,true);
        }
        if(value==1){
            QualitySettings.SetQualityLevel(2,true);
        }
        if(value==2){
            QualitySettings.SetQualityLevel(4,true);
        }
    }
    private void OnSoundToggleChange(bool value){

    }
    private void OnMusicToggleChange(bool value){
        
    }

    public override void OnBackBtnClick()
    {
        base.OnBackBtnClick();
        PlayerPrefs.SetFloat("RotateSpeed",rotateSpeedSlider.value);
        PlayerPrefs.SetInt("GraphicQuality",graphicsQuality.value);
        if(soundToggle.isOn){
            PlayerPrefs.SetInt("IsSound",1);
        }else{
            PlayerPrefs.SetInt("IsSound",0);
        }
        if(musicToggle.isOn){
            PlayerPrefs.SetInt("IsMusic",1);
        }else{
            PlayerPrefs.SetInt("IsMusic",0);
        }
        PlayerPrefs.Save();
#if UNITY_EDITOR
        // Cursor.lockState = CursorLockMode.Locked;
#endif
    }
}
