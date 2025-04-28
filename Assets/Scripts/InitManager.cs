using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class InitManager : Singleton<InitManager>
{
    // Start is called before the first frame update
    private async void Start()
    {
        Log("Initialization Started");
        await Initialization();
        Log("Initialization Completed");
    }
    private async UniTask Initialization()
    {
        try
        {
            UpdateLoadingUI(0.1f, "Firebase Init...");
            await UniTask.Delay(500);
            await InitFirebase();
            await UniTask.WaitUntil(() =>
                !FirebaseManager.Instance.IsFirebaseRemoteconfigEnable ||
                FirebaseManager.Instance.IsFetchRemoteConfigFinish);

            Debug.Log("Fetch Remote Config Finished");
            UpdateLoadingUI(0.4f, "Initializing...");
            await UniTask.Delay(500);
            await UniTask.DelayFrame(1);

            UpdateLoadingUI(1f, "Loading Scene...");
            await UniTask.Delay(500);
            OnLoadingDone();

            // InitIAPManager();
            // await UniTask.WaitUntil(() => IAPManager.Instance != null);

            // UpdateLoadingUI(0.4f, "Initializing...");

            // UpdateLoadingUI(0.5f, "Initializing...");
            // FirebaseManager.LoadingSceneEvent();
            // LoadScene().Forget();

        }
        catch (Exception e)
        {
            Log($"Initialization Failed: {e.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private async UniTask InitFirebase()
    {
        Log("InitFirebase", Color.cyan);
        FirebaseManager.Instance.Init();
        await UniTask.WaitUntil(() => FirebaseManager.Instance.IsInitProcessFinished);
        Log("InitFirebase Finished", Color.cyan);
    }
    private void Log(string value){
        Debug.Log(value);
    }
    private void Log(string value,Color color){
        Debug.Log(value);
    }
    private void UpdateLoadingUI(float value, string state){
        if(LoadingScreenController.Instance!=null){
            LoadingScreenController.Instance.UpdateLoading(value,state);
        }
    }
    private void OnLoadingDone(){
        if(LoadingScreenController.Instance!=null){
            LoadingScreenController.Instance.OnFinishLoading();
        }
    }
}
