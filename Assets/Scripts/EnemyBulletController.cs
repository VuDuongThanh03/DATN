using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider bulletCollider;
    Rigidbody bulletRigidbody;
    private float dameValue;
    private bool isDamged;
    void Awake()
    {
        bulletRigidbody = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        bulletCollider = gameObject.GetComponent<BoxCollider>();
    }
    public void SetDameValue(float value){
        dameValue = value;
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }
}

