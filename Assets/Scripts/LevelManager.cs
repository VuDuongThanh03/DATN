using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] LevelConfig levelConfig;
    public int GetSceneIndex(int levelIndex){
        foreach (var item in levelConfig.levelDatas)
        {
            if(item.LevelIndex==levelIndex){
                return item.SceneIndex;
            }
        }
        return -1;
    }
    public void NextLevel(){
        int currentSceneIndex = GetSceneIndex(GameManager.Instance.GameModel.CurrentLevel);
        int nextSceneIndex = GetSceneIndex(GameManager.Instance.GameModel.CurrentLevel+1);
        if(nextSceneIndex>=0){
            if(QuickLoadingController.Instance!=null){
                QuickLoadingController.Instance.ShowLoading();
            }
            SceneManager.UnloadSceneAsync(currentSceneIndex);
            SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Additive);
            GameManager.Instance.GameModel.SetCurrentLevel(GameManager.Instance.GameModel.CurrentLevel+1);
            GameManager.Instance.GameModel.SaveData();
        }else{
            PopupManager.Instance.GetPopup("PopupVictory");
            GameManager.Instance.GameModel.ClearSaveGame();
        }
    }
    public void PlayAgain(Action callBack = null){
        if(GameManager.Instance.GameModel.CurrentGameMode==0){
            if(QuickLoadingController.Instance!=null){
                QuickLoadingController.Instance.ShowLoading();
            }
            GameManager.Instance.GameModel.RollBackDataCheckPoint();
            int currentSceneIndex = GetSceneIndex(GameManager.Instance.GameModel.CurrentLevel);
            if(currentSceneIndex>=0){
                ReloadScene(currentSceneIndex);
            }
            if(callBack!=null){
                callBack?.Invoke();
            }
        }
        if(GameManager.Instance.GameModel.CurrentGameMode==1){
            if(QuickLoadingController.Instance!=null){
                QuickLoadingController.Instance.ShowLoading();
            }
            int currentSceneIndex = GetSceneIndex(GameManager.Instance.GameModel.CurrentLevel);
            SceneManager.UnloadSceneAsync(currentSceneIndex);
            int firstSceneIndex = GetSceneIndex(1);
            SceneManager.LoadScene(firstSceneIndex,LoadSceneMode.Additive);
            GameManager.Instance.GameModel.SetCurrentLevel(1);
            GameManager.Instance.GameModel.SetSaveState(true);
            GameManager.Instance.GameModel.SaveData();
            if(callBack!=null){
                callBack?.Invoke();
            }
        }
    }
    public async Task ReloadScene(int sceneIndex){
        await SceneManager.UnloadSceneAsync(sceneIndex);
        await SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
    }
}
