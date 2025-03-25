using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider boxCollider;
    Rigidbody arrowRigidbody;
    private float dameValue;
    private bool isDamged;
    void Awake()
    {
        arrowRigidbody = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }
    public void SetDameValue(float value){
        dameValue = value;
    }

    // Update is called once per frame
    void Update()
    {
        if(arrowRigidbody.isKinematic){
            return;
        }
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward , out RaycastHit hit,1f))
        {
            if(hit.collider.CompareTag("Player")){
                return;
            }
            arrowRigidbody.velocity = Vector3.zero;
            arrowRigidbody.isKinematic = true;
            GameObject GameObjecHit = hit.collider.gameObject;
            if(GameObjecHit!=null&&GameObjecHit.GetComponent<IDamageable>()!=null){
                GameObjecHit.GetComponent<IDamageable>().TakeDame(dameValue);
                isDamged = true;
                gameObject.SetActive(false);
                // gameObject.transform.SetParent(GameObjecHit.transform);
            }
        }
    }
}
