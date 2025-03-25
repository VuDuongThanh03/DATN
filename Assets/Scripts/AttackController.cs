using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using DATN;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public enum Weapon{
    SWORD,
    BOW
}
public class AttackController : MonoBehaviour
{
    private MyControllInputs _input;
    private float duationClick = 0 ; 
    [SerializeField] private CinemachineVirtualCamera aimCam;
    [SerializeField] private GameObject LeftWeaponShield;
    [SerializeField] private GameObject RightWeaponSword;
    [SerializeField] private GameObject LeftWeaponBow;
    [SerializeField] private GameObject RightWeaponArrow;
    [SerializeField] private Animator _animator;
    [SerializeField] private MultiAimConstraint BowAim;
    private PlayerController playerController;
    Vector2 currentBlendValue;
    [SerializeField] private GameObject ArrowPrefab;
    [SerializeField] private Transform ArrowSpawn;
    private Weapon _currentWeapon = Weapon.SWORD;
    public Weapon CurrentWeapon => _currentWeapon;
    private bool _isSpinAttackNow;
    public bool IsSpinAttackNow => _isSpinAttackNow;
    private bool _isHoldToSpinAttack;
    // Start is called before the first frame update
    void Awake()
    {
    }
    void Start()
    {
        GameManager.Instance.SetAttackControler(this);
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
        if(_isHoldToSpinAttack){
            _animator.SetFloat("X",0);
            _animator.SetFloat("Y",0);
        }else{
            _animator.SetFloat("X",currentBlendValue.x);
            _animator.SetFloat("Y",currentBlendValue.y);
        }
        // _animator.SetFloat("X",_input.move.x);
        // _animator.SetFloat("Y",_input.move.y);
        Attack();
    }
    private void Attack()
    {
        if (_input.startAttack == true&&!_isSpinAttackNow)
        {
            if (_input.confirmAttack == false)
            {
                if (duationClick < 0.2 && duationClick + Time.deltaTime >= 0.2)
                {
                    if(_currentWeapon==Weapon.SWORD){
                        _isHoldToSpinAttack = true;
                        GameManager.Instance.PlayerMovementController.SetIsHoldToSpinAttack(true);
                    }
                    if(_currentWeapon == Weapon.BOW){
                        MainHud.Instance.SetActiveCrosshair(true);
                        GameManager.Instance.SetRotateSpeedBowAttack(true);
                        aimCam.enabled = true;
                        // LeftWeaponShield.SetActive(false);
                        // RightWeaponSword.SetActive(false);
                        // LeftWeaponBow.SetActive(true);
                        RightWeaponArrow.SetActive(true);
                        _animator.SetTrigger("StartAttackBow");
                        BowAim.weight = 1;
                    }
                }
                duationClick += Time.deltaTime;
            }
            else
            {
                if (duationClick <= 0.2)
                {
                    Debug.Log("Short click attack");
                    if (_currentWeapon == Weapon.SWORD)
                    {
                        _animator.SetTrigger("Attack");
                        _input.startAttack = false;
                        _input.confirmAttack = false;
                        duationClick = 0;
                        CheckAttack();
                    }
                    if (_currentWeapon == Weapon.BOW)
                    {
                        _input.startAttack = false;
                        _input.confirmAttack = false;
                        duationClick = 0;
                    }

                }
                else
                {
                    Debug.Log("Long hold trigger attack: " + duationClick);
                    if(_currentWeapon==Weapon.SWORD){
                        SpinAttack((int)(duationClick*1000));
                        _input.startAttack = false;
                        _input.confirmAttack = false;
                        duationClick = 0;
                        _isHoldToSpinAttack = false;
                        GameManager.Instance.PlayerMovementController.SetIsHoldToSpinAttack(false);
                    }
                    if (_currentWeapon == Weapon.BOW)
                    {
                        MainHud.Instance.SetActiveCrosshair(false);
                        GameManager.Instance.SetRotateSpeedBowAttack(false);
                        _animator.SetTrigger("EndAttackBow");
                        BowAim.weight = 0;
                        // LeftWeaponShield.SetActive(true);
                        // RightWeaponSword.SetActive(true);
                        // LeftWeaponBow.SetActive(false);
                        RightWeaponArrow.SetActive(false);
                        GameObject arrow = Instantiate(ArrowPrefab);
                        arrow.transform.position = ArrowSpawn.position;
                        arrow.transform.rotation = ArrowSpawn.rotation;
                        //Set dame cho mui ten
                        arrow.GetComponent<ArrowController>()?.SetDameValue(20f);

                        RaycastHit hit;
                        Camera cam = Camera.main;
                        Ray ray = cam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

                        // Vector3 targetPoint = new Vector3();
                        bool isHit = false;
                        if (Physics.Raycast(ray, out hit))
                        {
                            Debug.Log("HitPos: " + hit.point);
                            isHit = true;
                            Debug.DrawLine(ray.origin, hit.point, Color.green, 100f);
                        }

                        // Thêm lực đẩy
                        Rigidbody rb = arrow.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            if(isHit){
                                rb.velocity = (hit.point - arrow.transform.position).normalized * 50f; // Điều chỉnh tốc độ theo ý muốn
                                arrow.transform.forward = (hit.point - cam.gameObject.transform.position).normalized;
                                Debug.DrawLine(arrow.transform.position, hit.point, Color.red, 100f);
                            }else{
                                Vector3 fakeHit = ray.origin+ray.direction.normalized*100;
                                rb.velocity = (fakeHit - arrow.transform.position).normalized * 50f; // Điều chỉnh tốc độ theo ý muốn
                                arrow.transform.forward = (fakeHit - cam.gameObject.transform.position).normalized;
                                Debug.DrawLine(arrow.transform.position, fakeHit, Color.red, 100f);
                            }
                            
                        }

                        _input.startAttack = false;
                        _input.confirmAttack = false;
                        duationClick = 0;
                        aimCam.enabled = false;
                    }
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
    public void SwapWeapon(){
        if(_currentWeapon==Weapon.SWORD){
            LeftWeaponShield.SetActive(false);
            RightWeaponSword.SetActive(false);
            LeftWeaponBow.SetActive(true);
            // RightWeaponArrow.SetActive(true);
            _currentWeapon = Weapon.BOW;
        }else{
            LeftWeaponShield.SetActive(true);
            RightWeaponSword.SetActive(true);
            LeftWeaponBow.SetActive(false);
            RightWeaponArrow.SetActive(false);
            _currentWeapon = Weapon.SWORD;
        }
    }
    public async void SpinAttack(int time){
        _animator.SetBool("SpinAttack",true);
        _isSpinAttackNow = true;
        await Task.Delay(time);
        _animator.SetBool("SpinAttack",false);
        _isSpinAttackNow = false;
    }
}
