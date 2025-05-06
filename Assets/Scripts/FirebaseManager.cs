using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Firebase.Crashlytics;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] bool isDebug;
    [SerializeField] private bool FirebaseRemoteconfig;
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
    public void LogEvent(string name)
        {
            if (IsFirebaseActive)
            {
                Debug.Log($"Analytics | LogEvent {name}");
                FirebaseAnalytics.LogEvent(name);
            }
            else
                Debug.Log("Firebase is not Finish Process Init");
        }

        public void LogEvent(string name, params Parameter[] parameters)
        {
            if (IsFirebaseActive)
            {
                if (isDebug)
                {
                    List<string> list = new List<string>();
                    foreach (var param in parameters)
                        list.Add(param.ToString());
                    string values = string.Join(", ", list);

                    Debug.Log($"Analytics | LogEvent {name}, params: {values}");
                }

                FirebaseAnalytics.LogEvent(name, parameters);
            }
            else
                Debug.Log("Firebase is not Finish Process Init");
        }
        public static void EventEnemyDie(string enemyType, int level, int amountCurrentEnemy)
        {
            try
            {
                Firebase.Analytics.Parameter[] parameters = {
                new Firebase.Analytics.Parameter("enemy_type", enemyType),
                new Firebase.Analytics.Parameter("level_index", level),
                new Firebase.Analytics.Parameter("amount_current_enemy", amountCurrentEnemy)
                };

                if (parameters.Length > 0)
                {
                    FirebaseManager.Instance.LogEvent("enemy_die", parameters);
                }
            }
            catch (Exception)
            {
                Debug.LogError("enemy_die");
            }
        }
        public static void EventAddResource(string resourceType, string source,int amount, int amountCurrent)
        {
            try
            {
                Firebase.Analytics.Parameter[] parameters = {
                new Firebase.Analytics.Parameter("resource_type", resourceType),
                new Firebase.Analytics.Parameter("source", source),
                new Firebase.Analytics.Parameter("amount", amount),
                new Firebase.Analytics.Parameter("amount_current", amountCurrent)
                };

                if (parameters.Length > 0)
                {
                    FirebaseManager.Instance.LogEvent("add_resource", parameters);
                }
            }
            catch (Exception)
            {
                Debug.LogError("add_resource");
            }
        }
        public static void EventUseResource(string resourceType,int amount, int amountCurrent)
        {
            try
            {
                Firebase.Analytics.Parameter[] parameters = {
                new Firebase.Analytics.Parameter("resource_type", resourceType),
                new Firebase.Analytics.Parameter("amount", amount),
                new Firebase.Analytics.Parameter("amount_current", amountCurrent)
                };

                if (parameters.Length > 0)
                {
                    FirebaseManager.Instance.LogEvent("use_resource", parameters);
                }
            }
            catch (Exception)
            {
                Debug.LogError("use_resource");
            }
        }
        public static void EventFinishLevel(int levelIndex, float playTime)
        {
            try
            {
                Firebase.Analytics.Parameter[] parameters = {
                new Firebase.Analytics.Parameter("level_index", levelIndex),
                new Firebase.Analytics.Parameter("play_time", playTime),
                };

                if (parameters.Length > 0)
                {
                    FirebaseManager.Instance.LogEvent("finish_level", parameters);
                }
            }
            catch (Exception)
            {
                Debug.LogError("finish_level");
            }
        }
        public static void EventPlayerDie(int levelIndex, float playTime, int amountCurrentEnemy)
        {
            try
            {
                Firebase.Analytics.Parameter[] parameters = {
                new Firebase.Analytics.Parameter("level_index", levelIndex),
                new Firebase.Analytics.Parameter("play_time", playTime),
                new Firebase.Analytics.Parameter("amount_current_enemy", amountCurrentEnemy)
                };

                if (parameters.Length > 0)
                {
                    FirebaseManager.Instance.LogEvent("player_die", parameters);
                }
            }
            catch (Exception)
            {
                Debug.LogError("player_die");
            }
        }
        public static void EventVictory()
        {
            try
            {
                FirebaseManager.Instance.LogEvent("victory");
            }
            catch (Exception)
            {
                Debug.LogError("victory");
            }
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
        Debug.Log($"Crashlytics | Setting Crashlytics user identifier: {id}");
        Crashlytics.SetUserId(id);
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
    RemoteConfigValue _getValueTemp = new RemoteConfigValue();
    public RemoteConfigValue GetRemoteConfigValue(string key)
    {
        RemoteConfigValue.Clone(_getValueTemp, FirebaseRemoteConfig.DefaultInstance.GetValue(key));

        // check if string value is null or not a number
        if (string.IsNullOrEmpty(_getValueTemp.StringValue))
        {
            _getValueTemp.StringValue = "";
        }
        // // check if number is null
        // else if (Double.IsNaN(_getValueTemp.DoubleValue))
        // {
        //     _getValueTemp.SetNumber(0);
        // }

        return _getValueTemp;
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
public class RemoteConfigValue
{
    internal static Regex booleanTruePattern = new Regex("^(1|true|t|yes|y|on)$", RegexOptions.IgnoreCase);
    internal static Regex booleanFalsePattern = new Regex("^(0|false|f|no|n|off|)$", RegexOptions.IgnoreCase);

    public bool BooleanValue
    {
        get
        {
            string stringValue = StringValue;
            if (booleanTruePattern.IsMatch(stringValue))
            {
                return true;
            }
            if (booleanFalsePattern.IsMatch(stringValue))
            {
                return false;
            }
            throw new FormatException($"ConfigValue '{stringValue}' is not a boolean value");
        }
    }

    public IEnumerable<byte> ByteArrayValue => Data;

    public double DoubleValue
    {
        get
        {
            try
            {
                return Convert.ToDouble(StringValue, CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }
    }

    public long LongValue
    {
        get
        {
            try
            {
                return Convert.ToInt64(StringValue, CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }
    }
    // public long LongValue => Convert.ToInt64(StringValue, CultureInfo.InvariantCulture);

    public float FloatValue
    {
        get
        {
            try
            {
                return Convert.ToSingle(StringValue, CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }
    }
    // public float FloatValue => Convert.ToSingle(StringValue, CultureInfo.InvariantCulture);

    public int IntegerValue
    {
        get
        {
            try
            {
                return Convert.ToInt32(StringValue, CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }
    }
    // public int IntegerValue => Convert.ToInt32(StringValue, CultureInfo.InvariantCulture);

    public string StringValue
    {
        get
        {
            return Encoding.UTF8.GetString(Data);
        }
        set
        {
            Data = Encoding.UTF8.GetBytes(value);
        }
    }

    internal byte[] Data { get; set; }

    public ValueSource Source { get; internal set; }

    public bool isNAN()
    {
        if (Double.IsNaN(DoubleValue))
            return true;

        return false;
    }

    public void SetNumber(int number)
    {
        Data = BitConverter.GetBytes(number);
    }

    public static void Clone(RemoteConfigValue value, ConfigValue firebaseValue)
    {
        value.Data = Encoding.UTF8.GetBytes(firebaseValue.StringValue);
    }
}
