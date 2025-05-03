using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{
    Coin,
    HealthBottle,
    StaminaBottle,
    Arrow,

}
public class CollectableItem : MonoBehaviour,ICollectable,IInteractable
{
    [SerializeField] ItemType itemType;
    [SerializeField] int amount;
    [SerializeField] bool isInteract = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool OnInteract()
    {
        if(itemType==ItemType.Coin&&isInteract==false){
            GameManager.Instance.GameModel.IncreaseCoin(amount);
            gameObject.SetActive(false);
            isInteract=true;
            return true;
        }
        if(itemType==ItemType.HealthBottle&&isInteract==false){
            if(GameManager.Instance.GameModel.TryIncreaseItemHealth(amount)){
                gameObject.SetActive(false);
                isInteract=true;
                return true;
            }else{
                //action notify full inventory
                return false;
            }
        }
        if(itemType==ItemType.StaminaBottle&&isInteract==false){
            if(GameManager.Instance.GameModel.TryIncreaseItemStamina(amount)){
                gameObject.SetActive(false);
                isInteract=true;
                return true;
            }else{
                //action notify full inventory
                return false;
            }
        }
        if(itemType==ItemType.Arrow&&isInteract==false){
            if(GameManager.Instance.GameModel.TryIncreaseArrow(amount)){
                gameObject.SetActive(false);
                isInteract=true;
                return true;
            }else{
                //action notify full inventory
                return false;
            }
        }
        return false;
    }
    public void SetupCollectableItem(int amount){
        this.amount = amount;
    }
}
