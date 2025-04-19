using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeedBackMessageController : Singleton<FeedBackMessageController>
{
    [SerializeField] GameObject _handleMessage;
    [SerializeField] Image _backMessage;
    [SerializeField] TextMeshProUGUI _textMessage;
    Sequence sequence;
    public void SetMessage(string message){
        _textMessage.text = message;
        _handleMessage.SetActive(true);
        _textMessage.DOKill();
        _backMessage.DOKill();
        _textMessage.DOFade(1,0).OnComplete(()=>{
            _textMessage.DOFade(0,2).SetDelay(1f).OnComplete(()=>{
                _handleMessage.SetActive(false);
            });
        });
        _backMessage.DOFade(1,0).OnComplete(()=>{
            _backMessage.DOFade(0,2).SetDelay(1f);
        });
    }
}
