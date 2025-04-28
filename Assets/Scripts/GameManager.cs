using System.Collections;
using System.Collections.Generic;
using DATN;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private PlayerController _playerController;
    private AttackController _attackController;
    private PlayerEquipController _playerEquipController;
    private PlayerMovementController _playerMovementController;
    private float _ratioRotateSpeed = 2f;
    public PlayerController PlayerController => _playerController;
    public AttackController AttackController => _attackController;
    public PlayerEquipController PlayerEquipController => _playerEquipController;
    public PlayerMovementController PlayerMovementController => _playerMovementController;
    public float RatioRotateSpeed => _ratioRotateSpeed;
    public Camera MainCamera;
    public GameConfig _gameConfig;
    public GameModel GameModel;
    public float CurrentHealth => GameModel.CurrentHealth;
    public float CurrentArmor => GameModel.CurrentArmor;
    public float CurrentStamina => GameModel.CurrentStamina;
    public float CurrentAngryEnergy => GameModel.CurrentAngryEnergy;
    float countDown = 0;
    private const float MIN_ROTATE_SPEED = 1;
    private const float MAX_ROTATE_SPEED = 9;
    void LoadConfigs()
    {
        _gameConfig = GameConfig.Load();
    }
    void LoadGame()
    {
        GameModel = GameModel.Load(_gameConfig);
    }
    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;
        //turn of v-sync
        QualitySettings.vSyncCount = 0;
        
        DontDestroyOnLoad(gameObject);

        if (MainCamera == null)
            MainCamera = Camera.main;

        LoadConfigs();
        LoadGame();
    }
    private void Start()
    {
        if(PlayerPrefs.HasKey("RotateSpeed")){
            UpdateRotateSpeed();
        }
        if(PlayerPrefs.HasKey("GraphicQuality")){
            int graphicsQualityIndex = PlayerPrefs.GetInt("GraphicQuality");
            if(graphicsQualityIndex==0){
            QualitySettings.SetQualityLevel(0,true);
            }
            if(graphicsQualityIndex==1){
                QualitySettings.SetQualityLevel(2,true);
            }
            if(graphicsQualityIndex==2){
                QualitySettings.SetQualityLevel(4,true);
            }
        }
        if(PlayerPrefs.HasKey("IsSound")){
            bool isSound = PlayerPrefs.GetInt("IsSound")==1;
        }
        if(PlayerPrefs.HasKey("IsMusic")){
            bool isMusic = PlayerPrefs.GetInt("IsMusic")==1;
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)){
            PopupManager.Instance.GetPopup("PopupExample");
        }
        countDown+=Time.deltaTime;
        if(countDown>=1){
            OnOneSecond();
            countDown = 0;
        }
    }
    public void OnOneSecond(){
        GameModel.IncreaseStamina(_gameConfig.ReturnStaminaPerSecond);
    }
    public void SetRotateSpeedBowAttack(bool isStart){
        if(isStart){
            _ratioRotateSpeed *= 3;
            Debug.Log("Rotate Speed: "+_ratioRotateSpeed);
        }else{
            UpdateRotateSpeed();
        }
    }
    public void SetPlayerControler(PlayerController playerController){
        _playerController = playerController;
    }
    public void SetAttackControler(AttackController attackController){
        _attackController = attackController;
    }
    public void SetPlayerEquipControler(PlayerEquipController playerEquipController){
        _playerEquipController = playerEquipController;
    }
    public void SetPlayerMovementController(PlayerMovementController playerMovementController){
        _playerMovementController = playerMovementController;
    }
    public void UpdateRotateSpeed(){
        float value = 0;
        if(PlayerPrefs.HasKey("RotateSpeed")){
            value = PlayerPrefs.GetFloat("RotateSpeed");
        }
        _ratioRotateSpeed = (MAX_ROTATE_SPEED-MIN_ROTATE_SPEED)*value + MIN_ROTATE_SPEED;
        Debug.Log("Rotate Speed: "+_ratioRotateSpeed);
    }
}
