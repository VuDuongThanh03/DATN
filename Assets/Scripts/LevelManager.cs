using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    bool isCallNextLevel = false;
    public bool IsCallNextLevel => isCallNextLevel;
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
    public void ResetCallNextLevel()
    {
        isCallNextLevel = false;
    }
    public void NextLevel()
    {
        int currentSceneIndex = GetSceneIndex(GameManager.Instance.GameModel.CurrentLevel);
        int nextSceneIndex = GetSceneIndex(GameManager.Instance.GameModel.CurrentLevel + 1);
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.EventFinishLevel(GameManager.Instance.GameModel.CurrentLevel, GameManager.Instance.CurrentLevelController.PlayTime);
        }
        isCallNextLevel = true;
        if (GameManager.Instance.PlayerMovementController != null)
        {
            GameManager.Instance.PlayerMovementController.ResetLoopFootStep();
        }
        if (GameManager.Instance.AttackController != null)
        {
            GameManager.Instance.AttackController.ResetLoopSpinAttack();
        }
        if (nextSceneIndex >= 0)
        {
            if (QuickLoadingController.Instance != null)
            {
                QuickLoadingController.Instance.ShowLoading();
            }
            SceneManager.UnloadSceneAsync(currentSceneIndex);
            SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Additive);
            GameManager.Instance.GameModel.SetCurrentLevel(GameManager.Instance.GameModel.CurrentLevel + 1);
            GameManager.Instance.GameModel.SaveData();
        }
        else
        {
            PopupManager.Instance.GetPopup("PopupVictory");
            SoundManager.Instance.PlayMusic(SoundMusicID.SOUND_VICTORY_MUSIC);
            GameManager.Instance.GameModel.ClearSaveGame();
            if (FirebaseManager.Instance != null)
            {
                FirebaseManager.EventVictory();
            }
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
