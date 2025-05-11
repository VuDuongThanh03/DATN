using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUISound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.GetComponent<Button>()!=null){
            gameObject.GetComponent<Button>().onClick.AddListener(OnClickButton);
        }
    }
    void OnClickButton(){
        if(SoundManager.Instance!=null){
            SoundManager.Instance.PlaySoundFX(SoundFXID.SOUNDFX_Button_Click);
        }
    }
}
