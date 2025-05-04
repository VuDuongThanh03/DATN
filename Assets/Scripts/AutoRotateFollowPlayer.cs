using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FowardPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance!=null&&GameManager.Instance.PlayerController!=null){
            Vector3 direction = GameManager.Instance.PlayerController.gameObject.transform.position-gameObject.transform.position;
            gameObject.transform.forward = new Vector3(direction.x,0,direction.z).normalized;
        }
    }
}
