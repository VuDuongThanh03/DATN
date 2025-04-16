using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : PopupBase
{
    // Start is called before the first frame update
    [SerializeField] Button closeButton;
    [SerializeField] Button buyHealthButton;
    [SerializeField] Button buyStaminaButton;
    [SerializeField] Button buyArrowButton;
    [SerializeField] Button upgradeLevel1Button;
    [SerializeField] Button upgradeLevel2Button;
    [SerializeField] TextMeshProUGUI textPriceHealth;
    [SerializeField] TextMeshProUGUI textCurrentAmoutItemHealth;
    [SerializeField] TextMeshProUGUI textPriceStamina;
    [SerializeField] TextMeshProUGUI textCurrentAmoutItemStamina;
    [SerializeField] TextMeshProUGUI textPriceArrow;
    [SerializeField] TextMeshProUGUI textCurrentAmoutItemArrow;
    [SerializeField] TextMeshProUGUI textPriceUpgradeLevel1;
    [SerializeField] TextMeshProUGUI textPriceUpgradeLevel2;
    [SerializeField] GameObject HandleUpgrade1;
    [SerializeField] GameObject HandleUpgrade2;
    [SerializeField] GameObject HandleUpgrade3;
    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    public void Init()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(OnBackBtnClick);
        buyHealthButton.onClick.AddListener(OnClickBuyHealth);
        buyStaminaButton.onClick.AddListener(OnClickBuyStamina);
        buyArrowButton.onClick.AddListener(OnClickBuyArrow);
        upgradeLevel1Button.onClick.AddListener(OnClickUpgradeLevel1);
        upgradeLevel2Button.onClick.AddListener(OnClickUpgradeLevel2);
        InitPrice();
        UpdateCurrentItemAmout();
        UpdateCurrentUpgrade();
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
#endif
    }
        public void InitPrice(){
        textPriceHealth.text = TradeConfig.Load().HealthBottlePrice.ToString();
        textPriceStamina.text = TradeConfig.Load().StaminaBottlePrice.ToString();
        textPriceArrow.text = TradeConfig.Load().ArrowPrice.ToString();
        textPriceUpgradeLevel1.text = TradeConfig.Load().UpdateLevel1Price.ToString();
        textPriceUpgradeLevel2.text = TradeConfig.Load().UpdateLevel2Price.ToString();
    }
    public void UpdateCurrentItemAmout(){
        textCurrentAmoutItemHealth.text = GameManager.Instance.GameModel.CurrentItemHealth+"/"+GameConfig.Load().MaxCountItemHealth;
        textCurrentAmoutItemStamina.text = GameManager.Instance.GameModel.CurrentItemStamina+"/"+GameConfig.Load().MaxCountItemStamina;
        textCurrentAmoutItemArrow.text = GameManager.Instance.GameModel.CurrentArrow+"/"+GameConfig.Load().MaxArrow;
    }
    public void UpdateCurrentUpgrade(){
        if(GameManager.Instance.GameModel.CurrentEquipLevel==0){
            HandleUpgrade1.SetActive(true);
            HandleUpgrade2.SetActive(false);
            HandleUpgrade3.SetActive(false);
        }
        if(GameManager.Instance.GameModel.CurrentEquipLevel==1){
            HandleUpgrade1.SetActive(false);
            HandleUpgrade2.SetActive(true);
            HandleUpgrade3.SetActive(false);
        }
        if(GameManager.Instance.GameModel.CurrentEquipLevel==2){
            HandleUpgrade1.SetActive(false);
            HandleUpgrade2.SetActive(false);
            HandleUpgrade3.SetActive(true);
        }
    }

    public override void OnBackBtnClick()
    {
        base.OnBackBtnClick();
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }
    public void OnClickBuyHealth(){
        if(GameManager.Instance.GameModel.CurrentCoin>=TradeConfig.Load().HealthBottlePrice){
            if(GameManager.Instance.GameModel.TryIncreaseItemHealth(1)){
                GameManager.Instance.GameModel.DecreaseCoin(TradeConfig.Load().HealthBottlePrice);
                UpdateCurrentItemAmout();
            }
        }
    }
    public void OnClickBuyStamina(){
        if(GameManager.Instance.GameModel.CurrentCoin>=TradeConfig.Load().StaminaBottlePrice){
            if(GameManager.Instance.GameModel.TryIncreaseItemStamina(1)){
                GameManager.Instance.GameModel.DecreaseCoin(TradeConfig.Load().StaminaBottlePrice);
                UpdateCurrentItemAmout();
            }
        }
    }
    public void OnClickBuyArrow(){
        if(GameManager.Instance.GameModel.CurrentCoin>=TradeConfig.Load().ArrowPrice){
            if(GameManager.Instance.GameModel.TryIncreaseArrow(1)){
                GameManager.Instance.GameModel.DecreaseCoin(TradeConfig.Load().ArrowPrice);
                UpdateCurrentItemAmout();
            }
        }
    }
    public void OnClickUpgradeLevel1(){
        if(GameManager.Instance.GameModel.CurrentCoin>=TradeConfig.Load().UpdateLevel1Price){
            if(GameManager.Instance.GameModel.CurrentEquipLevel==0){
                GameManager.Instance.GameModel.SetEquipLevel(GameManager.Instance.GameModel.CurrentEquipLevel+1);
                UpdateCurrentUpgrade();
            }
        }
    }
    public void OnClickUpgradeLevel2(){
        if(GameManager.Instance.GameModel.CurrentCoin>=TradeConfig.Load().UpdateLevel2Price){
            if(GameManager.Instance.GameModel.CurrentEquipLevel==1){
                GameManager.Instance.GameModel.SetEquipLevel(GameManager.Instance.GameModel.CurrentEquipLevel+1);
                UpdateCurrentUpgrade();
            }
        }
    }
}
