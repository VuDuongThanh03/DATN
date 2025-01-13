using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DATN;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private MyControllInputs _input;
    private float duationClick = 0 ; 
    public CinemachineVirtualCamera aimCam;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _input = gameObject.GetComponent<MyControllInputs>();
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }
    private void Attack()
    {
        if (_input.startAttack == true)
        {
            if (_input.confirmAttack == false)
            {
                if (duationClick < 0.2 && duationClick + Time.deltaTime >= 0.2)
                {
                    aimCam.enabled = true;
                }
                duationClick += Time.deltaTime;
            }
            else
            {
                if (duationClick <= 0.2)
                {
                    Debug.Log("Short click attack");
                    _animator.SetTrigger("Attack");
                    _input.startAttack = false;
                    _input.confirmAttack = false;
                    duationClick = 0;
                    CheckAttack();
                }
                else
                {
                    Debug.Log("Long hold trigger attack: " + duationClick);
                    _input.startAttack = false;
                    _input.confirmAttack = false;
                    duationClick = 0;
                    aimCam.enabled = false;
                }
            }
        }
    }
    public void CheckAttack(){
        RaycastHit hit;
        Vector3 direction = gameObject.transform.forward;   // Hướng nhìn từ mắt
        // Debug.DrawRay(gameObject.transform.position, direction * 5f, Color.red, 1f);
        // Bắn raycast từ mắt về phía trước
        if (Physics.Raycast(gameObject.transform.position, direction, out hit, 0.6f))
        {
            Debug.Log("Đã trúng " + hit.collider.name);

            // Kiểm tra nếu đối tượng có component nhận damage
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDame(10);  // Gây 10 sát thương
            }
        }
        else
        {
            Debug.Log("Không trúng mục tiêu nào.");
        }
    }
}
