using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour,IDamageable
{
    enum State{
        IdleState,
        PatrolState,
        TagetState,

    }
    enum TriggerAnim{
        Idle,
        Walk,
        Run,
        Attack,
    }
    public EnemyStatsConfig _baseStats;
    public NavMeshAgent _navMeshAgent;
    private EnemyStats _currentStats;
    public Animator enemyAnimator;
    public Slider enemyHealthBar;
    Vector3 _targetPos;
    Vector3 _posSpawn;
    float _idleTime;
    float _attackCountDown = 0;
    TriggerAnim LastTriggerAnim;
    
    [SerializeField]private State _currentState;

    // Start is called before the first frame update
    void Start()
    {
        _currentState = State.IdleState;
        _currentStats = new EnemyStats();
        _currentStats.health = _baseStats.EnemyStats.health;
        _currentStats.armor = _baseStats.EnemyStats.armor;
        enemyHealthBar.maxValue = _baseStats.EnemyStats.health;
        enemyHealthBar.value = _baseStats.EnemyStats.health;
    }

    // Update is called once per frame
    void Update()
    {
        // if(gameObject.transform.position.x==_targetPos.x&&gameObject.transform.position.z==_targetPos.z){
        //     _targetPos = new Vector3();
        // }
        // if(_targetPos==null||_targetPos==Vector3.zero){
        //     int x = Random.Range(-10,10);
        //     int z = Random.Range(-10,10);
        //     _targetPos = _posSpawn+ new Vector3(x,0,z);
        //     _navMeshAgent.SetDestination(_targetPos);
        // }else{
        //     return;
        // }
        CheckPlayerTaget();
        if(_currentState == State.IdleState){
            _idleTime-=Time.deltaTime;
            if(LastTriggerAnim!=TriggerAnim.Idle){
                enemyAnimator.SetTrigger("Idle");
                LastTriggerAnim = TriggerAnim.Idle;
            }
            if(_idleTime<=0){
                GoToStatePatrol();
            }
        }
        if(_currentState == State.PatrolState){
            _navMeshAgent.speed = 1;
            if(LastTriggerAnim!=TriggerAnim.Walk){
                enemyAnimator.SetTrigger("Walk");
                LastTriggerAnim = TriggerAnim.Walk;
            }
            if(gameObject.transform.position.x==_targetPos.x&&gameObject.transform.position.z==_targetPos.z){
                int ranIdleTime = Random.Range(3,10);
                _idleTime = ranIdleTime;
                _currentState = State.IdleState;
                enemyAnimator.SetTrigger("Idle");
                LastTriggerAnim = TriggerAnim.Idle;
            }
        }
        if(_currentState == State.TagetState){
            _attackCountDown-=Time.deltaTime;
            if(GameManager.Player!=null){
                float distance = Vector2.Distance(new Vector2(gameObject.transform.position.x,gameObject.transform.position.z),new Vector2(GameManager.Player.gameObject.transform.position.x,GameManager.Player.gameObject.transform.position.z));
                if(distance>6){
                    GoToStatePatrol();
                }else{
                    if(distance<1.5f){
                        StopToAttack();
                        if(_attackCountDown<=0){
                            enemyAnimator.SetTrigger("Attack");
                            LastTriggerAnim = TriggerAnim.Attack;
                            GameManager.Player.TakeDame(10);
                            _attackCountDown = 5;
                            ContinueToPatrol();
                            enemyAnimator.SetTrigger("Idle");
                            LastTriggerAnim = TriggerAnim.Idle;
                            // _navMeshAgent.SetDestination(new Vector3(GameManager.Player.gameObject.transform.position.x,0,GameManager.Player.gameObject.transform.position.z));
                        }else{
                            if(LastTriggerAnim!=TriggerAnim.Idle){
                                enemyAnimator.SetTrigger("Idle");
                            }
                        }
                    }else{
                        ContinueToPatrol();
                        enemyAnimator.SetTrigger("Run");
                        LastTriggerAnim = TriggerAnim.Run;
                        _navMeshAgent.SetDestination(new Vector3(GameManager.Player.gameObject.transform.position.x,0,GameManager.Player.gameObject.transform.position.z));
                    }
                }
            }
        }
    }
    public void TakeDame(float dame)
    {
        _currentStats.health=Mathf.Clamp(_currentStats.health-(dame-(dame*(_currentStats.armor/100))),0f,_baseStats.EnemyStats.health);
        enemyHealthBar.value = _currentStats.health;
        Debug.Log("Take dame: "+ dame+" Current Health: "+_currentStats.health);
        if(_currentStats.health==0){
            Debug.Log("Enemy Die");
            gameObject.SetActive(false);
        }
    }
    public void GoToStatePatrol(){
        int x = Random.Range(-10, 10);
        int z = Random.Range(-10, 10);
        _targetPos = _posSpawn + new Vector3(x, 0, z);
        _navMeshAgent.SetDestination(_targetPos);
        _currentState = State.PatrolState;
        enemyAnimator.SetTrigger("Walk");
    }
    public void StopToAttack(){
        _navMeshAgent.isStopped = true;
    }
    public void ContinueToPatrol(){
        _navMeshAgent.isStopped = false;
    }
    public void CheckPlayerTaget(){
        if(GameManager.Player!=null){
            float distance = Vector2.Distance(new Vector2(gameObject.transform.position.x,gameObject.transform.position.z),new Vector2(GameManager.Player.gameObject.transform.position.x,GameManager.Player.gameObject.transform.position.z));
            if(distance<=5&&_currentState!=State.TagetState){
                _currentState = State.TagetState;
                _navMeshAgent.speed = 3;
                enemyAnimator.SetTrigger("Run");
            }
        }
    }
}
