using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] List<GameObject> listEnemyAlive;
    List<GameObject> listEnemyDied;
    public bool IsClearEnemy(){
        if(listEnemyAlive!=null&&listEnemyAlive.Count==0){
            return true;
        }
        return false;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnEnterRunOutTrigger(Collider collider){
        if(GameManager.Instance!=null&&GameManager.Instance.PlayerController!=null){
            if(collider.gameObject == GameManager.Instance.PlayerController.gameObject){
                if(IsClearEnemy()){
                    if(LevelManager.Instance!=null){
                        LevelManager.Instance.NextLevel();
                    }
                }
            }
        }
    }
}
