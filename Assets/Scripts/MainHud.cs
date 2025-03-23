using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHud : Singleton<MainHud>
{
    // Start is called before the first frame update
    public GameObject Crosshair;
    void Start()
    {
        Crosshair.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
