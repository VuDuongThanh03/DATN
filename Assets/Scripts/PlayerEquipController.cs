using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipController : MonoBehaviour
{
    //set 1
    [SerializeField] private GameObject body1;
    [SerializeField] private GameObject body2;
    [SerializeField] private GameObject body3;
    [SerializeField] private GameObject hat2;
    [SerializeField] private GameObject hat3;
    [SerializeField] private GameObject sword1;
    [SerializeField] private GameObject sword2;
    [SerializeField] private GameObject sword3;
    [SerializeField] private GameObject shield1;
    [SerializeField] private GameObject shield2;
    [SerializeField] private GameObject shield3;
    [SerializeField] private GameObject hair;
    [SerializeField] private GameObject bow;
    void Start()
    {
        GameManager.Instance.SetPlayerEquipControler(this);
        if(GameManager.Instance.GameModel.CurrentEquipLevel>0){
            body1.SetActive(false);
            hair.SetActive(false);
            sword1.SetActive(false);
            shield1.SetActive(false);
        }
        if(GameManager.Instance.GameModel.CurrentEquipLevel == 1){
            body2.SetActive(true);
            hat2.SetActive(true);
            sword2.SetActive(true);
            shield2.SetActive(true);
        }
        if(GameManager.Instance.GameModel.CurrentEquipLevel == 2){
            body3.SetActive(true);
            hat3.SetActive(true);
            sword3.SetActive(true);
            shield3.SetActive(true);
        }  
    }
    public void OnUpgradeEquip(){
        if(GameManager.Instance.GameModel.CurrentEquipLevel == 1){
            body1.SetActive(false);
            hair.SetActive(false);
            body2.SetActive(true);
            hat2.SetActive(true);
            if(GameManager.Instance.AttackController.CurrentWeapon==Weapon.SWORD){
                sword1.SetActive(false);
                shield1.SetActive(false);
                sword2.SetActive(true);
                shield2.SetActive(true);
            }
        }
        if(GameManager.Instance.GameModel.CurrentEquipLevel == 2){
            body2.SetActive(false);
            hat2.SetActive(false);
            body3.SetActive(true);
            hat3.SetActive(true);
            if(GameManager.Instance.AttackController.CurrentWeapon==Weapon.SWORD){
                sword2.SetActive(false);
                shield2.SetActive(false);
                sword3.SetActive(true);
                shield3.SetActive(true);
            }
        }  
    }
    public void OnSwapWeapon(Weapon weapon){
        if(weapon == Weapon.SWORD){
            if(GameManager.Instance.GameModel.CurrentEquipLevel==0){
                sword1.SetActive(true);
                shield1.SetActive(true);
                bow.SetActive(false);
            }
            if(GameManager.Instance.GameModel.CurrentEquipLevel==1){
                sword2.SetActive(true);
                shield2.SetActive(true);
                bow.SetActive(false);
            }
            if(GameManager.Instance.GameModel.CurrentEquipLevel==2){
                sword3.SetActive(true);
                shield3.SetActive(true);
                bow.SetActive(false);
            }
        }
        if(weapon == Weapon.BOW){
            if(GameManager.Instance.GameModel.CurrentEquipLevel==0){
                sword1.SetActive(false);
                shield1.SetActive(false);
                bow.SetActive(true);
            }
            if(GameManager.Instance.GameModel.CurrentEquipLevel==1){
                sword2.SetActive(false);
                shield2.SetActive(false);
                bow.SetActive(true);
            }
            if(GameManager.Instance.GameModel.CurrentEquipLevel==2){
                sword3.SetActive(false);
                shield3.SetActive(false);
                bow.SetActive(true);
            }
        }
    }
}
