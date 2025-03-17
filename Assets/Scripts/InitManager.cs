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
            // UpdateLoadingUI(0.1f, "Initializing...");
            // await InitFirebase();
            // await UniTask.WaitUntil(() =>
            //     !FirebaseManager.Instance.IsFirebaseRemoteconfigEnable ||
            //     FirebaseManager.Instance.IsFetchRemoteConfigFinish);

            // YLogger.Log("Fetch Remote Config Finished", Color.cyan);
            // FirebaseManager.LoadingRemoteConfigEvent();
            // UpdateLoadingUI(0.2f, "Initializing...");
            // await UniTask.DelayFrame(1);

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
}
