using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
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
    public bool IsFirebaseRemoteconfigEnable => FirebaseRemoteconfig;
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
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }
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

        if (FirebaseRemoteconfig)
            await InitRemoteConfig();

        onComplete?.Invoke();
    }

    #region Analytics - Tracking
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
    #endregion

    #region Crashlytics
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
    #endregion

    #region Remote Config

    private bool _isFetchRemoteConfigFinish = false;
    public bool IsFetchRemoteConfigFinish => _isFetchRemoteConfigFinish;
    protected async Task InitRemoteConfig()
    {
        // Set callback
        // SetRemoteConfigCallbacks(true);

        // await fetch values
        Debug.Log("Init Remote Config");
        _isFetchRemoteConfigFinish = false;
        await FecthRemoteConfigValues();

        await UniTask.WaitUntil(() => _isFetchRemoteConfigFinish);
        Debug.Log("Init Remote Config Done");
    }

    public async Task FecthRemoteConfigValues()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (isDebug)
                Debug.Log("Fetch remoteconfig failed cause NetworkReachability is Not reachable");

            // OnFetchValuesFail(RemoteConfigFetchFailReason.Error);
            return;
        }
#if !UNITY_EDITOR
            await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.FromSeconds(1)). //no cached
#else
        await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.FromSeconds(1)). //no cache for editor
#endif
            ContinueWithOnMainThread((task) =>
            {
                if (task.IsCompleted)
                {
                    var info = FirebaseRemoteConfig.DefaultInstance.Info;

                    switch (info.LastFetchStatus)
                    {
                        case LastFetchStatus.Success:
                            {
                                FirebaseRemoteConfig.DefaultInstance.ActivateAsync();

                                // Callback
                                if (isDebug)
                                    Debug.Log("Fetch remoteconfig success and Activated");
                                // OnFetchValuesSuccess?.Invoke();
                            }
                            break;

                        case LastFetchStatus.Failure:
                            {
                                switch (info.LastFetchFailureReason)
                                {
                                    case FetchFailureReason.Error:
                                        {
                                            // Callback
                                            // OnFetchValuesFail?.Invoke(RemoteConfigFetchFailReason.Error);
                                        }
                                        break;
                                    case FetchFailureReason.Throttled:
                                        {
                                            // Callback
                                            // OnFetchValuesFail?.Invoke(RemoteConfigFetchFailReason.Throttled);
                                        }
                                        break;
                                }
                                if (isDebug)
                                    Debug.Log("Fetch remoteconfig failed");
                            }
                            break;

                        case LastFetchStatus.Pending:
                            {
                                // Callback
                                if (isDebug)
                                    Debug.Log("Fetch remoteconfig pending");
                                // OnFetchValuesFail?.Invoke(RemoteConfigFetchFailReason.Pending);
                            }
                            break;
                    }

                    _isFetchRemoteConfigFinish = true;
                }
            });
    }
    #endregion
}
