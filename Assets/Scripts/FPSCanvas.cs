using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FPSCanvas : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI fpsText;
    [SerializeField] private float updateInterval = 0.5f;
    [SerializeField] private float _timeAvgFps = 600f;
    private float accum = 0.0f;
    private float elapsedTime = 0.0f;
    private int frames = 0;
    private int lowFpsCount = 0;
    private float timeleft;
    private float fps = 0.0f;

    private float _currentTimeAvgFPS = 0;
    private List<float> _lstFps;

    // Start is called before the first frame update
    void Start()
    {
        _currentTimeAvgFPS = _timeAvgFps;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTimeAvgFPS -= Time.unscaledDeltaTime;
        elapsedTime += Time.unscaledDeltaTime;
        ++frames;

        if (elapsedTime >= updateInterval)
        {
            fps = frames / elapsedTime;
            timeleft = updateInterval;
            frames = 0;
            elapsedTime = 0.0f;
            fpsText.text = "FPS: " + fps.ToString("f2");
            if(fps<20){
                ++lowFpsCount;
            }
            if (_lstFps == null)
            {
                _lstFps = new List<float>();
            }
            _lstFps.Add(fps);
            if (_currentTimeAvgFPS <= 0f)
            {
                _currentTimeAvgFPS = _timeAvgFps;
                float total = _lstFps.Sum(fps => fps);
                float avgFps = total / _lstFps.Count;
                float minFps = _lstFps.Min(fps => fps);
                float maxFps = _lstFps.Max(fps => fps);
                // FirebaseManager.UserStatsEvent(avgFps, minFps, maxFps,lowFpsCount,_lstFps.Count,_timeAvgFps);
                _lstFps.Clear();
                lowFpsCount = 0;
            }
        }
    }
}
