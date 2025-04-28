using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScreenController : Singleton<LoadingScreenController>
{
    [SerializeField] TMP_Text stateLoadingText;
    [SerializeField] Slider loadingSlider;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void UpdateLoading(float value,string state){
        // loadingSlider.value = 0;
        loadingSlider.DOValue(value,0.5f);
        stateLoadingText.text = state;
    }
    public void OnFinishLoading(){
        gameObject.SetActive(false);
    }
}
