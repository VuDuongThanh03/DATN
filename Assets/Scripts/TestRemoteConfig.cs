using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class TestRemoteConfig : MonoBehaviour
{
    // Start is called before the first frame update
    UnityEngine.UI.Image shape;
    void Awake(){
        CheckRemoteConfigValue();
    }
    void Start()
    {
        shape = gameObject.GetComponent<UnityEngine.UI.Image>();
    }

    private Task CheckRemoteConfigValue(){
        Debug.Log("Fetching data");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask){
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }
        remoteConfig.ActivateAsync()
          .ContinueWithOnMainThread(
            task => {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
                bool shapeHaveColor = remoteConfig.GetValue("Shape_Have_Color").BooleanValue;
                if(shapeHaveColor){
                    shape.color = Color.red;
                }else{
                    shape.color = Color.white;
                }

                // string configData = remoteConfig.GetValue("all_Game_data").StringValue;
                // allConfigData = JsonUtility.FromJson<ConfigData>(configData);

              /*  print("Total values: "+remoteConfig.AllValues.Count);

                foreach (var item in remoteConfig.AllValues)
                {
                    print("Key :" + item.Key);
                    print("Value: " + item.Value.StringValue);
                }*/

            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
