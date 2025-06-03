using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainHud : Singleton<MainHud>
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private Button swapWeaponBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private Button useItemHealthBtn;
    [SerializeField] private Button useItemStaminaBtn;
    [SerializeField] private Button interactBtn;
    [SerializeField] private Image iconWeaponButton;
    [SerializeField] private Image iconWeaponAvatar;
    [SerializeField] private Sprite iconSword;
    [SerializeField] private Sprite iconBow;
    [SerializeField] private Image imageAngryEnergy;
    [SerializeField] private GameObject resourceArrow;
    [SerializeField] private TextMeshProUGUI textCoin, textArrow, textItemHealth, textItemStamina;

    void Start()
    {
        interactBtn.gameObject.SetActive(false);
        useItemStaminaBtn.gameObject.SetActive(false);
        Crosshair.SetActive(false);
        swapWeaponBtn.onClick.AddListener(OnClickButtonSwapWeaponBtn);
        skillBtn.onClick.AddListener(OnClickButtonSkill);
        interactBtn.onClick.AddListener(OnClickButtonInteract);
        useItemHealthBtn.onClick.AddListener(OnClickButtonUseItemHealth);
        useItemStaminaBtn.onClick.AddListener(OnClickButtonUseItemStamina);
        SetupUI();
    }
    private void SetupUI(){
        //Button Skill
        if(GameManager.Instance.CurrentAngryEnergy==GameConfig.Load().MaxAngryEnergy){
            skillBtn.gameObject.SetActive(true);
        }else{
            skillBtn.gameObject.SetActive(false);
        }

        //Button Swap Weapon
        if(GameManager.Instance.GameModel.IsHaveBow){
            swapWeaponBtn.gameObject.SetActive(true);
            resourceArrow.SetActive(true);
        }else{
            swapWeaponBtn.gameObject.SetActive(false);
            resourceArrow.SetActive(false);
        }

        //Button use Item Health
        if(GameManager.Instance.GameModel.CurrentItemHealth>0&&GameManager.Instance.CurrentHealth<GameConfig.Load().HealthDefault){
            useItemHealthBtn.gameObject.SetActive(true);
        }else{
            useItemHealthBtn.gameObject.SetActive(false);
        }

        //Button use Item Stamina
        if(GameManager.Instance.GameModel.CurrentItemStamina>0&&GameManager.Instance.CurrentStamina<GameConfig.Load().StatminaDefault){
            useItemStaminaBtn.gameObject.SetActive(true);
        }else{
            useItemStaminaBtn.gameObject.SetActive(false);
        }
        UpdateResourceDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetActiveCrosshair(bool isActive){
        Crosshair.SetActive(isActive);
    }
    public void OnClickButtonSwapWeaponBtn(){
        GameManager.Instance.AttackController.SwapWeapon();
    }
    public void OnClickButtonSkill(){
        GameManager.Instance.AttackController.TryUseSkill();
    }
    public void OnClickButtonInteract(){
        GameManager.Instance.PlayerController.OnClickInteract();
    }
    public void OnClickButtonUseItemHealth(){
        GameManager.Instance.GameModel.DecreaseItemHealth(1);
        GameManager.Instance.GameModel.IncreaseHealth(GameConfig.Load().ReturnHealthValueUseItem);
    }
    public void OnClickButtonUseItemStamina(){
        GameManager.Instance.GameModel.DecreaseItemStamina(1);
        GameManager.Instance.GameModel.IncreaseStamina(GameConfig.Load().ReturnStaminaValueUseItem);
    }
    public void SetActiveButtonSkill(bool active){
        skillBtn.gameObject.SetActive(active);
    }
    public void ChangeAvatarWeapon(Weapon weapon){
        if(weapon == Weapon.SWORD){
            iconWeaponButton.sprite = iconSword;
            iconWeaponAvatar.sprite = iconSword;
        }else{
            iconWeaponButton.sprite = iconBow;
            iconWeaponAvatar.sprite = iconBow;
        }
    }
    public void OnAngryEnergyChange(){
        imageAngryEnergy.fillAmount = GameManager.Instance.CurrentAngryEnergy/GameConfig.Load().MaxAngryEnergy;
        if(GameManager.Instance.CurrentAngryEnergy==GameConfig.Load().MaxAngryEnergy){
            skillBtn.gameObject.SetActive(true);
        }else{
            skillBtn.gameObject.SetActive(false);
        }
    }
    public void OnUnlockBow(){
        swapWeaponBtn.gameObject.SetActive(true);
        resourceArrow.SetActive(true);
    }
    public void CheatUnlockBow(){
        GameManager.Instance.GameModel.UnlockBow();
    }
    public void CheatAddCoin(){
        GameManager.Instance.GameModel.IncreaseCoin(50);
    }
    public void CheatKillAll()
    {
        GameManager.Instance.CurrentLevelController.KillAllEnemy();
    }
    public void CheatNextLevel()
    {
        LevelManager.Instance.NextLevel();
    }
    public void CheatFullAngry()
    {
        GameManager.Instance.GameModel.IncreaseAngryEnergy(100);
    }
    public void SetActiveInteractButton(bool active)
    {
        interactBtn.gameObject.SetActive(active);
    }
    public void SetActiveUseItemHealthButton(bool active){
        useItemHealthBtn.gameObject.SetActive(active);
    }
    public void SetActiveUseStaminaItemButton(bool active){
        useItemStaminaBtn.gameObject.SetActive(active);
    }
    public void UpdateResourceDisplay(){
        textCoin.text = GameManager.Instance.GameModel.CurrentCoin.ToString();
        string temp = GameManager.Instance.GameModel.CurrentArrow+"/"+GameConfig.Load().MaxArrow;
        textArrow.text = temp;
        temp = GameManager.Instance.GameModel.CurrentItemHealth+"/"+GameConfig.Load().MaxCountItemHealth;
        textItemHealth.text = temp;
        temp = GameManager.Instance.GameModel.CurrentItemStamina+"/"+GameConfig.Load().MaxCountItemStamina;
        textItemStamina.text = temp;
    }
    public void OnClickPause(){
        PopupManager.Instance?.GetPopup("PopupPause");
    }
}
