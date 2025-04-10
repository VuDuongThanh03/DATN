using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHud : Singleton<MainHud>
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private Button swapWeaponBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private Image iconWeaponButton;
    [SerializeField] private Image iconWeaponAvatar;
    [SerializeField] private Sprite iconSword;
    [SerializeField] private Sprite iconBow;
    [SerializeField] private Image imageAngryEnergy;


    void Start()
    {
        Crosshair.SetActive(false);
        swapWeaponBtn.onClick.AddListener(OnClickButtonSwapWeaponBtn);
        skillBtn.onClick.AddListener(OnClickButtonSkill);
        if(GameManager.Instance.CurrentAngryEnergy==GameConfig.Load().MaxAngryEnergy){
            skillBtn.gameObject.SetActive(true);
        }else{
            skillBtn.gameObject.SetActive(false);
        }

        if(GameManager.Instance.GameModel.IsHaveBow){
            swapWeaponBtn.gameObject.SetActive(true);
        }else{
            swapWeaponBtn.gameObject.SetActive(false);
        }
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
    }
    public void OnUnlockBow(){
        swapWeaponBtn.gameObject.SetActive(true);
    }
    public void CheatUnlockBow(){
        GameManager.Instance.GameModel.UnlockBow();
    }
}
