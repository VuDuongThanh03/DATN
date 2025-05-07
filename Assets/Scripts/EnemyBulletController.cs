using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletController : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider bulletCollider;
    Rigidbody bulletRigidbody;
    private float dameValue;
    private bool isDamged;
    [SerializeField] private float lifeTime = 5f;
    private float _timer;
    private IObjectPool<EnemyBulletController> myPool;
    void Awake()
    {
        bulletRigidbody = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        _timer = lifeTime;
        bulletCollider = gameObject.GetComponent<BoxCollider>();
    }
    public void SetPool(IObjectPool<EnemyBulletController> pool)
    {
        myPool = pool;
    }
    public void SetDameValue(float value){
        dameValue = value;
    }

    // Update is called once per frame
    void Update()
    {
        _timer-=Time.deltaTime;
        if(bulletRigidbody.isKinematic){
            return;
        }
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward , out RaycastHit hit,0.5f))
        {
            if(hit.collider.CompareTag("Enemy")||hit.collider.CompareTag("CollectableItem")){
                return;
            }
            bulletRigidbody.velocity = Vector3.zero;
            bulletRigidbody.isKinematic = true;
            GameObject GameObjecHit = hit.collider.gameObject;
            if(GameObjecHit!=null&&GameObjecHit.GetComponent<IDamageable>()!=null){
                GameObjecHit.GetComponent<IDamageable>().TakeDame(dameValue);
                isDamged = true;
                // gameObject.transform.SetParent(GameObjecHit.transform);
            }
            gameObject.SetActive(false);
            myPool.Release(this);
        }
        if(_timer<=0){
            myPool.Release(this);
        }
    }
    public void ResetBullet(){
        isDamged = false;
        _timer = lifeTime;
        bulletRigidbody.isKinematic = false;
    }
}

