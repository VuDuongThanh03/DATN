using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Comparers;

public class EnemyController : MonoBehaviour,IDamageable
{
    public EnemyStatsConfig _baseStats;
    public NavMeshAgent _navMeshAgent;
    private EnemyStats _currentStats;
    Vector3 _targetPos;
    Vector3 _posSpawn;

    // Start is called before the first frame update
    void Start()
    {
        _currentStats = new EnemyStats();
        _currentStats.health = _baseStats.EnemyStats.health;
        _currentStats.armor = _baseStats.EnemyStats.armor;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.x==_targetPos.x&&gameObject.transform.position.z==_targetPos.z){
            _targetPos = new Vector3();
        }
        if(_targetPos==null||_targetPos==Vector3.zero){
            int x = Random.Range(-10,10);
            int z = Random.Range(-10,10);
            _targetPos = _posSpawn+ new Vector3(x,0,z);
            _navMeshAgent.SetDestination(_targetPos);
        }else{
            return;
        }
    }
    public void TakeDame(float dame)
    {
        _currentStats.health=Mathf.Clamp(_currentStats.health-(dame-(dame*(_currentStats.armor/100))),0f,_baseStats.EnemyStats.health);
        Debug.Log("Take dame: "+ dame+" Current Health: "+_currentStats.health);
        if(_currentStats.health==0){
            Debug.Log("Enemy Die");
            gameObject.SetActive(false);
        }
    }
}
