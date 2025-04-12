using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
    // Start is called before the first frame update
    // public CharacterStatsConfig _baseStats;
    // private CharacterStats _currentStats;
    private GameConfig _gameConfig;
    private bool _playerIsDie;
    private Animator _animator;
    public bool PlayerIsDie => _playerIsDie;
    public Action OnPlayerDie;
    private List<GameObject> interactableObjects;
    private GameObject lastedObjectTriggerPlayer;
    void Awake()
    {
        interactableObjects = new List<GameObject>();
    }
    void Start()
    {
        _gameConfig = GameConfig.Load();
        GameManager.Instance.SetPlayerControler(this);
        _animator = gameObject.GetComponent<Animator>();
        PlayerHealthBar.Instance.SetupHealthBar(GameManager.Instance.CurrentHealth,_gameConfig.HealthDefault);
        PlayerStaminaBar.Instance.SetupStaminaBar(GameManager.Instance.CurrentStamina,_gameConfig.StatminaDefault);
        MainHud.Instance.OnAngryEnergyChange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDame(float dame){
        GameManager.Instance.GameModel.SetHealth(Mathf.Clamp(GameManager.Instance.CurrentHealth-(dame-(dame*(GameManager.Instance.CurrentArmor/100))),0f,_gameConfig.HealthDefault));
        GameManager.Instance.GameModel.IncreaseAngryEnergy(dame*2);
        Debug.Log("Player take dame: "+ dame+" Current Health: "+GameManager.Instance.CurrentHealth);
        if(GameManager.Instance.CurrentHealth>0&&dame>0){
            _animator.SetTrigger("TakeDame");
        }
        if(GameManager.Instance.CurrentHealth==0){
            Debug.Log("Player Die");
            _playerIsDie = true;
            _animator.SetTrigger("Die");
            OnPlayerDie?.Invoke();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<IInteractable>()!=null){
            MainHud.Instance.SetActiveInteractButton(true);
            if(!interactableObjects.Contains(other.gameObject)){
                interactableObjects.Add(other.gameObject);
                lastedObjectTriggerPlayer = other.gameObject;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<IInteractable>()!=null){
            if(interactableObjects.Contains(other.gameObject)){
                interactableObjects.Remove(other.gameObject);
                if(other.gameObject==lastedObjectTriggerPlayer){
                    lastedObjectTriggerPlayer=null;
                    if(interactableObjects.Count>0){
                        lastedObjectTriggerPlayer = interactableObjects[interactableObjects.Count-1];
                    }
                }
            }
            if(interactableObjects.Count==0){
                MainHud.Instance.SetActiveInteractButton(false);
            }
        }
    }
    public void OnClickInteract(){
        if(lastedObjectTriggerPlayer!=null&&lastedObjectTriggerPlayer.GetComponent<IInteractable>()!=null){
            IInteractable interactable = lastedObjectTriggerPlayer.GetComponent<IInteractable>();
            interactable.OnInteract();
        }
    }
}
