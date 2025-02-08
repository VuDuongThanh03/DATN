using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class TestFirebase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Testing FireBase");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task=>{
            Debug.Log("Turn on SetAnalyticsCollectionEnabled Start");
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            Debug.Log("Turn on SetAnalyticsCollectionEnabled Complete");
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void TestTracking(){
        FirebaseAnalytics.LogEvent("ClickButton");
        Debug.Log("tracking click button");
    }
}
