using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class QuickLoadingController : Singleton<QuickLoadingController>
{
    // Start is called before the first frame update
    [SerializeField] GameObject handleLoading;
    [SerializeField] Slider loadingSlider;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public async Task ShowLoading(){
        loadingSlider.value = 0;
        handleLoading.SetActive(true);
        loadingSlider.DOValue(UnityEngine.Random.Range(0.1f,0.8f),0.5f).OnComplete(()=>{
            loadingSlider.DOValue(1,UnityEngine.Random.Range(0.5f,1f)).OnComplete(()=>{
                handleLoading.SetActive(false);
            });
        });
    }
}
