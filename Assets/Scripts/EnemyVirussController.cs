using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;

public class EnemyVirussController : MonoBehaviour,IDamageable,IDropable
{
    enum State{
        IdleState,
        PatrolState,
        TagetState,
        Die,

    }
    enum TriggerAnim{
        Idle,
        Walk,
        Run,
        Attack,
        TakeDame,
        Die,
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
    float _countDownTakeTime = 0;
    float _countDownDespawn = 0;
    TriggerAnim LastTriggerAnim;
    
    [SerializeField]private State _currentState;
    [SerializeField]private GameObject _bulletPrefab;


    // Start is called before the first frame update
    void Start()
    {
        _posSpawn = gameObject.transform.position;
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
        if(_currentState==State.Die){
            _countDownDespawn-=Time.deltaTime;
            if(_countDownDespawn<=0){
                gameObject.SetActive(false);
            }
            return;
        }
        if(LastTriggerAnim==TriggerAnim.TakeDame){
            _countDownTakeTime-=Time.deltaTime;
            if(_countDownTakeTime>0){
                return;
            }
            ContinueToPatrol();
        }
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
            if(_navMeshAgent.isStopped){
                _navMeshAgent.isStopped = false;
            }
            _navMeshAgent.speed = 1;
            if(LastTriggerAnim!=TriggerAnim.Walk){
                enemyAnimator.SetTrigger("Walk");
                LastTriggerAnim = TriggerAnim.Walk;
            }
            if(/*Mathf.Abs(gameObject.transform.position.x-_targetPos.x)<=0.001&&Mathf.Abs(gameObject.transform.position.z-_targetPos.z)<=0.001*/_navMeshAgent.remainingDistance<=0.001){
                if(_navMeshAgent.pathPending == false){
                    int ranIdleTime = Random.Range(3,10);
                    _idleTime = ranIdleTime;
                    _currentState = State.IdleState;
                    enemyAnimator.SetTrigger("Idle");
                    LastTriggerAnim = TriggerAnim.Idle;
                }
            }
        }
        if(_currentState == State.TagetState){
            _attackCountDown-=Time.deltaTime;
            if(GameManager.Instance.PlayerController!=null){
                float distance = Vector2.Distance(new Vector2(gameObject.transform.position.x,gameObject.transform.position.z),new Vector2(GameManager.Instance.PlayerController.gameObject.transform.position.x,GameManager.Instance.PlayerController.gameObject.transform.position.z));
                if(distance>=10){
                    GoToStatePatrol();
                }else{
                    if(distance<10f){
                        StopMove();
                        if(_attackCountDown<=0){
                            enemyAnimator.SetTrigger("Attack");
                            LastTriggerAnim = TriggerAnim.Attack;
                            StartCoroutine(WaitForAnimation(1,0.5f));
                            _attackCountDown = 3;
                            ContinueToPatrol();
                            enemyAnimator.SetTrigger("Idle");
                            LastTriggerAnim = TriggerAnim.Idle;
                            // _navMeshAgent.SetDestination(new Vector3(GameManager.Instance.PlayerController.gameObject.transform.position.x,0,GameManager.Instance.PlayerController.gameObject.transform.position.z));
                        }else{
                            enemyAnimator.SetTrigger("Idle");
                            LastTriggerAnim = TriggerAnim.Idle;
                            gameObject.transform.forward = (GameManager.Instance.PlayerController.gameObject.transform.position-gameObject.transform.position).normalized;
                        }
                    }
                }
            }
        }
    }
    public void TakeDame(float dame,Weapon weapon = Weapon.SWORD)
    {
        if(_currentState==State.Die){
            return;
        }
        if(weapon==Weapon.SWORD){
            if(GameManager.Instance.GameModel.CurrentEquipLevel==0){
            SoundManager.Instance.PlaySoundFX(SoundFXID.SOUNDFX_Sword_Wood_Hit);
            }else{
                SoundManager.Instance.PlaySoundFX(SoundFXID.SOUNDFX_Sword_Metal_Hit);
            }
        }
        // SoundManager.Instance.PlaySoundFXDelay(SoundFXID.SOUNDFX_Enemy_Hurt,300);
        _currentStats.health=Mathf.Clamp(_currentStats.health-(dame-(dame*(_currentStats.armor/100))),0f,_baseStats.EnemyStats.health);
        enemyHealthBar.value = _currentStats.health;
        Debug.Log("Take dame: "+ dame+" Current Health: "+_currentStats.health);
        if(_currentStats.health>0&&dame>0){
            StopMove();
            _countDownTakeTime = 1f;
            enemyAnimator.ResetTrigger("Run");
            enemyAnimator.ResetTrigger("Idle");
            enemyAnimator.SetTrigger("TakeDame");
            LastTriggerAnim = TriggerAnim.TakeDame;
        }
        if(_currentStats.health==0){
            if(GameManager.Instance!=null&&GameManager.Instance.CurrentLevelController!=null){
                GameManager.Instance.CurrentLevelController.OnEnemyDie(gameObject);
                if(FirebaseManager.Instance!=null){
                    FirebaseManager.EventEnemyDie("Virus",GameManager.Instance.GameModel.CurrentLevel,GameManager.Instance.CurrentLevelController.GetAmountCurrentEnemy());
                }
            }
            StopMove();
            Debug.Log("Enemy Die");
            _currentState = State.Die;
            enemyAnimator.SetTrigger("Die");
            _countDownDespawn = 5;
            GameManager.Instance.GameModel.IncreaseAngryEnergy(20);
            enemyHealthBar.gameObject.SetActive(false);
            Drop();
        }
    }
    public void GoToStatePatrol(){
        int x = Random.Range(-5, 5);
        int z = Random.Range(-5, 5);
        _targetPos = _posSpawn + new Vector3(x, 0, z);
        _navMeshAgent.SetDestination(_targetPos);
        _currentState = State.PatrolState;
        enemyAnimator.ResetTrigger("Idle");
        enemyAnimator.SetTrigger("Walk");
    }
    public void StopMove(){
        _navMeshAgent.isStopped = true;
    }
    public void ContinueToPatrol(){
        _navMeshAgent.isStopped = false;
    }
    IEnumerator WaitForAnimation(float dame,float time)
    {
        yield return new WaitForSeconds(time);
        //not pool
        // GameObject bullet = Instantiate(_bulletPrefab);
        // Rigidbody rb = bullet.GetComponent<Rigidbody>();
        // bullet.transform.position = gameObject.transform.GetChild(0).position;
        // bullet.transform.forward = (GameManager.Instance.PlayerController.gameObject.transform.position+new Vector3(0f,0.5f,0f)-bullet.transform.position).normalized;
        // bullet.GetComponent<EnemyBulletController>()?.SetDameValue(20f);
        //not pool
        //use pool
        var bullet = EnemyBulletPool.Instance.GetBullet();
        bullet.ResetBullet();
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.transform.position = gameObject.transform.GetChild(0).position;
        bullet.transform.forward = (GameManager.Instance.PlayerController.gameObject.transform.position+new Vector3(0f,0.5f,0f)-bullet.transform.position).normalized;
        bullet.GetComponent<EnemyBulletController>()?.SetDameValue(20f);
        //use pool
        if(rb!=null){
            rb.velocity = (GameManager.Instance.PlayerController.gameObject.transform.position+new Vector3(0f,0.5f,0f)-bullet.transform.position).normalized * 15f;
        }
        SoundManager.Instance.PlaySoundFX(SoundFXID.SOUNDFX_Viruss_Shot);
        // GameManager.Instance.PlayerController.TakeDame(dame);
        // ContinueToPatrol();
    }
 
    public void CheckPlayerTaget(){
        if(GameManager.Instance.PlayerController!=null&&!GameManager.Instance.PlayerController.PlayerIsDie){
            float distance = Vector2.Distance(new Vector2(gameObject.transform.position.x,gameObject.transform.position.z),new Vector2(GameManager.Instance.PlayerController.gameObject.transform.position.x,GameManager.Instance.PlayerController.gameObject.transform.position.z));
            if(distance<=10&&_currentState!=State.TagetState){
                if(GameManager.Instance.PlayerController!=null){
                    GameManager.Instance.PlayerController.OnPlayerDie-=GoToStatePatrol;
                    GameManager.Instance.PlayerController.OnPlayerDie+=GoToStatePatrol;
                }
                _currentState = State.TagetState;
                StopMove();
                gameObject.transform.forward = (GameManager.Instance.PlayerController.gameObject.transform.position-gameObject.transform.position).normalized;
                // _navMeshAgent.speed = 3;
                // enemyAnimator.SetTrigger("Run");
            }
        }
    }

    public void Drop()
    {
        int ran = Random.Range(0, 2);
        if(ran<1){
            return;
        }
        DropItemData dropItemData = new DropItemData();
        DropItemConfig dropItemConfig = DropItemConfig.Load();
        int sumWeight = 0;
        if(dropItemConfig!=null&&dropItemConfig.dropItemDatas!=null&&dropItemConfig.dropItemDatas.Count>0){
            // sumWeight = dropItemConfig.dropItemDatas.Sum(x=>x.weight);
            foreach (var item in dropItemConfig.dropItemDatas)
            {
                if(item.itemType==ItemType.Arrow&&GameManager.Instance.GameModel.IsHaveBow==false){
                    continue;
                }
                sumWeight+=item.weight;
            }
            int ranNumber = Random.Range(1,sumWeight+1);
            foreach (var item in dropItemConfig.dropItemDatas)
            {
                ranNumber-=item.weight;
                if(ranNumber<=0){
                    dropItemData = item;
                    break;
                }
            }
        }
        if(dropItemData!=null){
            GameObject itemDrop = Instantiate(dropItemData.prefabDropItem);
            int ranQuantity = Random.Range(dropItemData.minQuantity,dropItemData.maxQuantity);
            itemDrop.GetComponent<CollectableItem>()?.SetupCollectableItem(ranQuantity);
            itemDrop.transform.position = gameObject.transform.position;
            int navMeshLayer = LayerMask.GetMask("NavMesh");
            Ray ray = new Ray(itemDrop.transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f, navMeshLayer))
            {
                itemDrop.transform.position = hit.point;
            }
        }
    }
}
