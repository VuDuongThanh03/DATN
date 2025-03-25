using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHud : Singleton<MainHud>
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private Button swapWeaponBtn; 
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
}
