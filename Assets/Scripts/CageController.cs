using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;

public class CageController : MonoBehaviour,IDamageable
{
    public Slider enemyHealthBar;
    public float CageHealthDefault = 200;
    public float CageArmor = 0;
    public GameObject fx;
    float CurrentCageHealth;


    // Start is called before the first frame update
    void Start()
    {
        CurrentCageHealth = CageHealthDefault;
        enemyHealthBar.maxValue = CageHealthDefault;
        enemyHealthBar.value = CageHealthDefault;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDame(float dame,Weapon weapon = Weapon.SWORD)
    {
        if(GameManager.Instance!=null&&GameManager.Instance.CurrentLevelController!=null){
            if(GameManager.Instance.CurrentLevelController.IsClearEnemy()==false){
                FeedBackMessageController.Instance.SetMessage("You need destroy all enemy first");
                return;
            }
        }
        CurrentCageHealth = Mathf.Clamp(CurrentCageHealth-(dame-(dame*(CageArmor/100))),0f,CageHealthDefault);
        enemyHealthBar.value = CurrentCageHealth;
        Debug.Log("Take dame: "+ dame+" Current Cage Health: " + CurrentCageHealth);
        SoundManager.Instance.PlaySoundFX(SoundFXID.SOUNDFX_Attack_Cage);
        if(CurrentCageHealth==0){
            SoundManager.Instance.PlaySoundFX(SoundFXID.SOUNDFX_Cage_Broken);
            if(GameManager.Instance!=null&&GameManager.Instance.CurrentLevelController!=null){
                //Ready for interact and end game
                GameManager.Instance.CurrentLevelController.SetReadyForEnd();
            }
            fx.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
