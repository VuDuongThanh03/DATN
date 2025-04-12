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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnInteract()
    {
        if(itemType==ItemType.Coin){
            GameManager.Instance.GameModel.IncreaseCoin(amount);
        }
        if(itemType==ItemType.HealthBottle){
            if(GameManager.Instance.GameModel.TryIncreaseItemHealth(amount)){
                gameObject.SetActive(false);
            }else{
                //action notify full inventory
            }
        }
        if(itemType==ItemType.HealthBottle){
            if(GameManager.Instance.GameModel.TryIncreaseItemHealth(amount)){
                gameObject.SetActive(false);
            }else{
                //action notify full inventory
            }
        }
        if(itemType==ItemType.StaminaBottle){
            if(GameManager.Instance.GameModel.TryIncreaseItemStamina(amount)){
                gameObject.SetActive(false);
            }else{
                //action notify full inventory
            }
        }
        if(itemType==ItemType.Arrow){
            if(GameManager.Instance.GameModel.TryIncreaseArrow(amount)){
                gameObject.SetActive(false);
            }else{
                //action notify full inventory
            }
        }
    }
}
