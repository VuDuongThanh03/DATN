using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class NPCCongChuaController : MonoBehaviour,IInteractable
{
    // Start is called before the first frame update
    [SerializeField] GameObject messageHandle;
    [SerializeField] int countDownMessage = 8;
    [SerializeField] TMP_Text messageText;
    float currentCountDown = 0;
    void Start()
    {
        if(GameManager.Instance!=null&&GameManager.Instance.CurrentLevelController!=null){
            GameManager.Instance.CurrentLevelController.OnReadyForEnd += OnReadyForEnd;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.CurrentLevelController.IsReadyForEnd){
            return;
        }
        currentCountDown-=Time.deltaTime;
        if(currentCountDown<=0){
            if(GameManager.Instance!=null&&GameManager.Instance.PlayerController!=null){
                if(Vector3.Distance(gameObject.transform.position,GameManager.Instance.PlayerController.gameObject.transform.position)>5){
                    return;
                }
            }
            messageHandle.SetActive(true);
            AutoOffMessage();
            currentCountDown = countDownMessage;
        }
        
    }
    public bool OnInteract()
    {
        if(GameManager.Instance!=null&&GameManager.Instance.CurrentLevelController!=null){
            if(GameManager.Instance.CurrentLevelController.IsReadyForEnd){
                ShowMessageAndFinish();
            }
        }
        return false;
    }
    public async Task AutoOffMessage(){
        await UniTask.Delay(3000);
        messageHandle.SetActive(false);
    }
    public async Task ShowMessageAndFinish(){
        await UniTask.Delay(500);
        messageText.SetText("Thank you so much for rescuing me.");
        messageHandle.SetActive(true);
        await UniTask.Delay(3000);
        LevelManager.Instance.NextLevel();
    }
    public void OnReadyForEnd(){
        messageHandle.SetActive(false);
        GameManager.Instance.CurrentLevelController.OnReadyForEnd -= OnReadyForEnd;
    }
}
