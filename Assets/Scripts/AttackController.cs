using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DATN;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AttackController : MonoBehaviour
{
    private MyControllInputs _input;
    private float duationClick = 0 ; 
    public CinemachineVirtualCamera aimCam;
    public GameObject LeftWeaponShield;
    public GameObject RightWeaponSword;
    public GameObject LeftWeaponBow;
    public GameObject RightWeaponArrow;
    private Animator _animator;
    public MultiAimConstraint BowAim;
    private PlayerController playerController;
    Vector2 currentBlendValue;
    // Start is called before the first frame update
    void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        _input = gameObject.GetComponent<MyControllInputs>();
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // sử dụng nội suy để chuyển anim mượt mà thông qua nội suy giá trị x,y khi chuyển hướng
        if(playerController.PlayerIsDie){
            BowAim.weight=0;
            aimCam.enabled = false;
            return;
        }
        currentBlendValue = Vector2.Lerp(currentBlendValue, _input.move, Time.deltaTime * 8f);
        _animator.SetFloat("X",currentBlendValue.x);
        _animator.SetFloat("Y",currentBlendValue.y);
        // _animator.SetFloat("X",_input.move.x);
        // _animator.SetFloat("Y",_input.move.y);
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
                    LeftWeaponShield.SetActive(false);
                    RightWeaponSword.SetActive(false);
                    LeftWeaponBow.SetActive(true);
                    RightWeaponArrow.SetActive(true);
                    _animator.SetTrigger("StartAttackBow");
                    BowAim.weight=1;
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
                    _animator.SetTrigger("EndAttackBow");
                    BowAim.weight=0;
                    LeftWeaponShield.SetActive(true);
                    RightWeaponSword.SetActive(true);
                    LeftWeaponBow.SetActive(false);
                    RightWeaponArrow.SetActive(false);
                    _input.startAttack = false;
                    _input.confirmAttack = false;
                    duationClick = 0;
                    aimCam.enabled = false;
                }
            }
        }
    }
    public void CheckAttack(){
        GameObject temp;
        List<GameObject> listObjectTakeDame = new List<GameObject>();
        
        Vector3 forward = gameObject.transform.forward;
        temp = CheckRayAttack(forward,Color.red);
        if(temp!=null){
            listObjectTakeDame.Add(temp);
        }
        Vector3 leftDirection = Quaternion.AngleAxis(-20, Vector3.up) * forward;  // Lệch trái
        temp = CheckRayAttack(leftDirection,Color.green,listObjectTakeDame);
        if(temp!=null){
            listObjectTakeDame.Add(temp);
        }
        // Vector3 rightDirection = Quaternion.AngleAxis(20, Vector3.up) * forward; 
        // CheckRayAttack(rightDirection,Color.yellow);
        // Bắn raycast từ mắt về phía trước
        
    }
    public GameObject CheckRayAttack(Vector3 direction, Color color,List<GameObject> checkObject = null){
        RaycastHit hit;
        Debug.DrawRay(gameObject.transform.position + new Vector3(0,0.8f,0), direction*1f, color, 10f);
        if (Physics.Raycast(gameObject.transform.position + new Vector3(0,0.8f,0), direction , out hit, 0.8f))
        {
            Debug.Log("Đã trúng " + hit.collider.name);

            // Kiểm tra nếu đối tượng có component nhận damage
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                GameObject hitGameObject = hit.collider.gameObject;
                if(checkObject!=null){
                    foreach(var item in checkObject){
                        if(item == hitGameObject){
                            return null;
                        }
                    }
                }
                damageable.TakeDame(10);  // Gây 10 sát thương
                return hit.collider.gameObject;
            }
        }
        else
        {
            Debug.Log("Không trúng mục tiêu nào.");
        }
        return null;
    }
}
