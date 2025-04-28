using UnityEngine;

public class ScalerShopHandel : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake(){
        CheckAndSet();
    }
    void Start()
    {
    }

    // Update is called once per frame
    public void CheckAndSet(){
        if(((float)Screen.width/Screen.height)<((float)16/9)){
            float value = (float)((float)Screen.width/Screen.height)/((float)16/9);
            gameObject.GetComponent<RectTransform>().localScale = new Vector3((float)value,(float)value,(float)value); 
        }
    }
}
