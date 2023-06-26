using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class wolfattack : LivingEntity, IDamageable
{
    public GameObject fence;
    public GameObject[] targetfences; //여러 울타리 타켓
    private Vector3 wolfpos;

    private NavMeshAgent navigation;
    private Animator wolfAnimator; // 애니메이터 컴포넌트
    private GameObject closefence;

    public static bool isfence = false; //여러개의 fence중 하나만 부서짐
    public static bool isfencestop = false; //여러개의 fence중 하나만 부서짐

    private int foxchildObjectsLen;
    public static bool isfox = false;
    public static bool isfoxfox = false; //여러개의 fence중 하나만 부서짐

    public GameObject fox;
    public GameObject[] foxchildObjects;
    public GameObject closefox;

    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점

    public Slider wolf_healthSlider; // 체력을 표시할 UI 슬라이더
    private RabbitManager rabbitManager; // RabbitManager 스크립트에 대한 참조

    private void Awake()
    {
        wolfpos = transform.position;
        navigation = GetComponent<NavMeshAgent>();
        wolfAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        fence = GameObject.FindWithTag("Fences");
        fox = GameObject.FindWithTag("Rabbits");
        rabbitManager = GameObject.FindWithTag("Rabbits").GetComponent<RabbitManager>();

        //fox 자식 배열 생성
        int childCount = fox.transform.childCount;
        foxchildObjectsLen = childCount;
        foxchildObjects = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            Transform childtrans = fox.transform.GetChild(i);
            if (childtrans.gameObject != null)
            {
                foxchildObjects[i] = childtrans.gameObject;
            }
        }
        //fence 자식 배열 생성
        int childCount_fence = fence.transform.childCount;
        targetfences = new GameObject[childCount_fence];
        for (int i = 0; i < childCount_fence; i++)
        {
            Transform childtrans = fence.transform.GetChild(i);
            if (childtrans.gameObject != null)
            {
                targetfences[i] = childtrans.gameObject;
            }
        }
        if (targetfences != null)
        {
            wherebrokenfence(); //늑대와 가장 가까운 울타리 제거 
        }
    }

    private void Update()
    {
        wolfpos = transform.position;
        if (isfencestop == true && isfox == false && foxchildObjectsLen >= 0 && isfoxfox == false && foxchildObjects != null)
        {
            wherefox();
        }
        if (isfox == true)
        {
            isfox = false;
        }
    }

    private void wherebrokenfence()
    {
        float distance = Mathf.Infinity; // 초기 거리 무한
        closefence = targetfences[0];
        if (isfence == false)
        {
            foreach (GameObject target_fence in targetfences)
            {
                if (target_fence != null)
                {
                    float dis = Vector3.Distance(wolfpos, target_fence.transform.position);
                    if (distance < dis)
                    {
                        distance = dis;
                        closefence = target_fence;
                    }
                }
            }
            navigation.SetDestination(closefence.transform.position);
        }
        isfence = true;
    }

    private void wherefox()
    {
        foreach (GameObject target_fox in foxchildObjects)
        {
            if (target_fox != null)
            {
                closefox = target_fox;
            }
        }

        navigation.SetDestination(closefox.transform.position);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("wolf collider : " + other);
        if (other.CompareTag("Fence"))
        {
            Trigger_fuc(other);
        }
        else if (isfencestop == true && other.CompareTag("Rabbit"))
        {
            Trigger_fuc(other);
        }
    }

    private void Trigger_fuc(Collider other)
    {
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity attackTarget
                = other.GetComponent<LivingEntity>();
            Debug.Log("wolf attack : " + attackTarget);
            if (attackTarget != null)
            {
                lastAttackTime = Time.time;

                Vector3 hitPoint
                    = other.ClosestPoint(transform.position);
                Vector3 hitNormal
                    = transform.position - other.transform.position;
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
                Debug.Log("wolf attack : " + attackTarget);
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        wolf_healthSlider.maxValue = startingHealth;
        wolf_healthSlider.value = health;
    }

    public override void OnDamage(float damage, Vector3 hitPoint,
        Vector3 hitDirection)
    {

        base.OnDamage(damage, hitPoint, hitDirection);
        Debug.Log("Wolf on Damage ");
        wolf_healthSlider.value = health;
    }

    // 사망 처리
    public override void Die()
    {
        base.Die();
        wolf_healthSlider.gameObject.SetActive(false);

        gameObject.SetActive(false);
        Destroy(gameObject);
        rabbitManager.CreateBag();
    }

}