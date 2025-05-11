using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DATN;
using EPOOutline;
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
    EquipmentStatsConfig equipmentStatsConfig;
    void Awake()
    {
        interactableObjects = new List<GameObject>();
    }
    void Start()
    {
        _gameConfig = GameConfig.Load();
        equipmentStatsConfig = EquipmentStatsConfig.Load();
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
    public void TakeDame(float dame,Weapon weapon = Weapon.SWORD){
        float armor = equipmentStatsConfig.GetEquipmentStat(GameManager.Instance.GameModel.CurrentEquipLevel).armor;
        GameManager.Instance.GameModel.SetHealth(Mathf.Clamp(GameManager.Instance.CurrentHealth-(dame-(dame*(armor/100))),0f,_gameConfig.HealthDefault));
        GameManager.Instance.GameModel.IncreaseAngryEnergy(dame*2);
        Debug.Log("Player take dame: "+ dame+" Current Health: "+GameManager.Instance.CurrentHealth);
        if(GameManager.Instance.CurrentHealth>0&&dame>0){
            SoundManager.Instance.PlaySoundFX(SoundFXID.SOUNDFX_Player_Hurt);
            _animator.SetTrigger("TakeDame");
        }
        if(GameManager.Instance.CurrentHealth==0){
            if(!_playerIsDie){
                OnCharacterDie();
            }
            Debug.Log("Player Die");
            _playerIsDie = true;
            _animator.SetTrigger("Die");
            OnPlayerDie?.Invoke();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BossAxe")){
            TakeDame(10);
        }
        if(other.CompareTag("RatSword")){
            TakeDame(10);
        }
        if(other.gameObject.GetComponent<IInteractable>()!=null){
            MainHud.Instance.SetActiveInteractButton(true);
            if(!interactableObjects.Contains(other.gameObject)){
                interactableObjects.Add(other.gameObject);
                if(lastedObjectTriggerPlayer!=null&&lastedObjectTriggerPlayer.GetComponent<Outlinable>()!=null){
                    lastedObjectTriggerPlayer.GetComponent<Outlinable>().enabled = false;
                }
                lastedObjectTriggerPlayer = other.gameObject;
                if(lastedObjectTriggerPlayer!=null&&lastedObjectTriggerPlayer.GetComponent<Outlinable>()!=null){
                    lastedObjectTriggerPlayer.GetComponent<Outlinable>().enabled = true;
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<IInteractable>()!=null){
            if(interactableObjects.Contains(other.gameObject)){
                interactableObjects.Remove(other.gameObject);
                if(other.gameObject==lastedObjectTriggerPlayer){
                    if(lastedObjectTriggerPlayer!=null&&lastedObjectTriggerPlayer.GetComponent<Outlinable>()!=null){
                        lastedObjectTriggerPlayer.GetComponent<Outlinable>().enabled = false;
                    }
                    lastedObjectTriggerPlayer=null;
                    if(interactableObjects.Count>0){
                        lastedObjectTriggerPlayer = interactableObjects[interactableObjects.Count-1];
                        if(lastedObjectTriggerPlayer!=null&&lastedObjectTriggerPlayer.GetComponent<Outlinable>()!=null){
                            lastedObjectTriggerPlayer.GetComponent<Outlinable>().enabled = true;
                        }
                    }
                }
            }
            if(interactableObjects.Count==0){
                MainHud.Instance.SetActiveInteractButton(false);
            }
        }
    }
    void OnRemoveCollectableObject(GameObject objectCollected){
        if(objectCollected.GetComponent<IInteractable>()!=null){
            if(interactableObjects.Contains(objectCollected)){
                interactableObjects.Remove(objectCollected);
                if(objectCollected==lastedObjectTriggerPlayer){
                    if(lastedObjectTriggerPlayer!=null&&lastedObjectTriggerPlayer.GetComponent<Outlinable>()!=null){
                        lastedObjectTriggerPlayer.GetComponent<Outlinable>().enabled = false;
                    }
                    lastedObjectTriggerPlayer=null;
                    if(interactableObjects.Count>0){
                        lastedObjectTriggerPlayer = interactableObjects[interactableObjects.Count-1];
                        if(lastedObjectTriggerPlayer!=null&&lastedObjectTriggerPlayer.GetComponent<Outlinable>()!=null){
                            lastedObjectTriggerPlayer.GetComponent<Outlinable>().enabled = true;
                        }
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
            bool interactSuccess = interactable.OnInteract();
            if(lastedObjectTriggerPlayer.GetComponent<ICollectable>()!=null){
                if(interactSuccess){
                    OnRemoveCollectableObject(lastedObjectTriggerPlayer);
                }
            }
            
        }
    }
    public async Task OnCharacterDie(){
        SoundManager.Instance.StopMusic();
        if(GameManager.Instance.PlayerMovementController!=null){
            GameManager.Instance.PlayerMovementController.ResetLoopFootStep();
        }
        if(GameManager.Instance.AttackController!=null){
            GameManager.Instance.AttackController.ResetLoopSpinAttack();
        }
        SoundManager.Instance.PlaySoundFX(SoundFXID.SOUNDFX_Lose);
        await UniTask.Delay(2000);
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.EventPlayerDie(GameManager.Instance.GameModel.CurrentLevel, GameManager.Instance.CurrentLevelController.PlayTime,GameManager.Instance.CurrentLevelController.GetAmountCurrentEnemy());
        }
        PopupManager.Instance.GetPopup("PopupLose");
        if(GameManager.Instance.GameModel.CurrentGameMode==1){
            GameManager.Instance.GameModel.SetDataPlayAgain();
        }
    }
}
