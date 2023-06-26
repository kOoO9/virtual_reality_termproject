using UnityEngine;

public class RabbitMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 50f;
    public float idleTime = 3f;
    public float runTime = 2f;

    private Animator animator;
    private bool isMoving;
    private float idleTimer;
    private float runTimer;
    private bool isRotating;
    private bool isDead;

    private Rigidbody rb;
    private GameObject[] fences;
    private BoxCollider rabbitCollider;
    private Collider[] fenceColliders; // 울타리 충돌체들을 저장하는 배열

    void Start()
    {
        animator = GetComponent<Animator>();
        isDead = GetComponent<RabbitHp>().isDead;
        if (animator == null)
        {
            Debug.LogError("Animator 컴포넌트를 찾을 수 없습니다.");
        }

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody 컴포넌트를 찾을 수 없습니다.");
        }

        rabbitCollider = GetComponent<BoxCollider>();

        fences = GameObject.FindGameObjectsWithTag("Fence");
        if (fences == null || fences.Length == 0)
        {
            Debug.LogError("Fence 태그가 설정된 오브젝트를 찾을 수 없습니다.");
        }

        // 울타리 충돌체 배열 초기화
        fenceColliders = new Collider[fences.Length];
        for (int i = 0; i < fences.Length; i++)
        {
            fenceColliders[i] = fences[i].GetComponent<BoxCollider>();
        }


        // 초기화
        isMoving = false;
        idleTimer = 0f;
        runTimer = 0f;
        isRotating = false;

        // 처음에는 idle 애니메이션을 재생
        animator.SetTrigger("Idle");
    }

    void Update()
    {
        fences = GameObject.FindGameObjectsWithTag("Fence");
        if (fences == null || fences.Length == 0)
        {
            Debug.LogError("Fence 태그가 설정된 오브젝트를 찾을 수 없습니다.");
        }

        // 울타리 충돌체 배열 초기화
        fenceColliders = new Collider[fences.Length];
        for (int i = 0; i < fences.Length; i++)
        {
            fenceColliders[i] = fences[i].GetComponent<BoxCollider>();
        }

        isDead = GetComponent<RabbitHp>().isDead;
        if (isDead)
        {
            animator.SetTrigger("Dead");
            Destroy(gameObject, 5);
        }
        else
        {
            // idle 상태인 경우
            if (!isMoving)
            {
                // 일정 시간이 지나면 움직임 시작
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleTime)
                {
                    StartRunning();
                }
            }
            // run 상태인 경우
            else
            {
                // 일정 시간이 지나면 움직임 종료
                runTimer += Time.deltaTime;
                if (runTimer >= runTime)
                {
                    StopRunning();
                }
            }

            // 움직임 및 회전 적용
            if (isMoving)
            {
                Move();
                CheckCollision();
            }
            if (isRotating)
            {
                Rotate();
            }
        }
        
    }

    void StartRunning()
    {
        isMoving = true;
        idleTimer = 0f;
        runTimer = 0f;
        isRotating = false;

        // run 애니메이션 재생
        animator.SetTrigger("Run");
    }

    void StopRunning()
    {
        isMoving = false;
        idleTimer = 0f;
        runTimer = 0f;
        isRotating = false;

        // idle 애니메이션 재생
        animator.SetTrigger("Idle");
    }

    void Move()
    {
        // rabbit 오브젝트를 앞으로 이동
        Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void CheckCollision()
    {
        // 울타리 충돌 검사
        for (int i = 0; i < fenceColliders.Length; i++)
        {
            if (rabbitCollider.bounds.Intersects(fenceColliders[i].bounds))
            {
                // 충돌 감지 시 회전
                isRotating = true;
            }
        }
    }

    void Rotate()
    {
        StopRunning();
        // 충돌 감지 시 회전
        // 랜덤한 회전 각도 생성
        float randomAngle = Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0f, randomAngle, 0f);
        //Quaternion targetRotation = Quaternion.LookRotation(-transform.forward, transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, randomRotation, rotationSpeed * Time.deltaTime);
        StartRunning();
    }
}
