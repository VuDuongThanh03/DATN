using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    // Start is called before the first frame update
    [SerializeField] private GameObject playerStartSpawnPoint;
    public GameObject PlayerStartSpawnPoint => playerStartSpawnPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnPlayer(){

    }
}
