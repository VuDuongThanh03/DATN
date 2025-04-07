using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHud : Singleton<MainHud>
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private Button swapWeaponBtn;
    [SerializeField] private Image iconWeaponButton;
    [SerializeField] private Image iconWeaponAvatar;
    [SerializeField] private Sprite iconSword;
    [SerializeField] private Sprite iconBow;


    void Start()
    {
        Crosshair.SetActive(false);
        swapWeaponBtn.onClick.AddListener(OnClickButtonSwapWeaponBtn);
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
    public void ChangeAvatarWeapon(Weapon weapon){
        if(weapon == Weapon.SWORD){
            iconWeaponButton.sprite = iconSword;
            iconWeaponAvatar.sprite = iconSword;
        }else{
            iconWeaponButton.sprite = iconBow;
            iconWeaponAvatar.sprite = iconBow;
        }
    }
}
