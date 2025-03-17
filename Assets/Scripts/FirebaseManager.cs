using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] bool isDebug;
    [SerializeField] private bool FirebaseRemoteconfig;
    [SerializeField] private bool FirebaseFirestore;
    [SerializeField] private bool FirebaseMessage;
    bool _isInitInProcess = true;
    public bool IsInitProcessFinished => !_isInitInProcess;
    DependencyStatus _status = DependencyStatus.UnavailableDisabled;
    public bool IsInitSuccess()
    {
        return _status == DependencyStatus.Available;
    }
    public bool IsFirebaseActive => IsInitSuccess();
    #region Singleton
        static FirebaseManager _instance;
        public static FirebaseManager Instance
        {
            get
            {
                return _instance;
            }
        }
    #endregion
    void Start()
    {
        
    }
    public void Init()
    {
        if (!IsInitSuccess())
        {
            _isInitInProcess = true;

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                _status = task.Result;
                if (_status == DependencyStatus.Available)
                {
                    InitServices(onComplete: () =>
                    {
                        if (isDebug)
                            Debug.Log("Init Success");

                        // Callback
                        // OnInitSuccess?.Invoke();

                        _isInitInProcess = false;
                    });
                }
                else
                {
                    if (isDebug)
                        Debug.Log("Init dependencies failed, status : " + _status);

                    // Callback
                    // OnInitFail?.Invoke();

                    _isInitInProcess = false;
                }
            });
        }
    }
    async void InitServices(Action onComplete)
        {
            //default services - all project has this service enable
            InitAnalytics();
            InitCrashytics();

            // if (FirebaseRemoteconfig)
            //     await InitRemoteConfig();

            onComplete?.Invoke();
        }
    // Update is called once per frame
    protected void InitAnalytics()
        {
            Debug.Log("Analytics | Init");

            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            // Set the user ID.
            FirebaseAnalytics.SetUserId(SystemInfo.deviceUniqueIdentifier);

            // Set default session duration values.
            //FirebaseAnalytics.SetMinimumSessionDuration(new TimeSpan(0, 0, 10));
            FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        }
        protected void InitCrashytics()
        {
            Debug.Log("Crashlytics | Init");
            FirebaseApp.LogLevel = LogLevel.Error;
            SetUserIdCrashlytic(SystemInfo.deviceUniqueIdentifier);
        }
        private void SetUserIdCrashlytic(String id)
        {
            // Debug.Log($"Crashlytics | Setting Crashlytics user identifier: {id}");
            // Crashlytics.SetUserId(id);
        }
    void Update()
    {
        
    }
}
