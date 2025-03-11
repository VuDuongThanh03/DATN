using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider boxCollider;
    Rigidbody arrowRigidbody;
    void Awake()
    {
        arrowRigidbody = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
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
        }
    }
}
