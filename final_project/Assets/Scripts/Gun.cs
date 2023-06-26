using System.Collections;
using UnityEngine;

// 총을 구현한다
public class Gun : LivingEntity {
    public enum State {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장전 중
    }

    public State state { get; private set; } // 현재 총의 상태

    public Transform fireTransform; // 총알이 발사될 위치
    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기

    public float damage = 25; // 공격력
    private float fireDistance = 50f; // 사정거리

    public float timeBetFire = 0.12f; // 총알 발사 간격
    public float reloadTime = 1.8f; // 재장전 소요 시간
    private float lastFireTime; // 총을 마지막으로 발사한 시점


    private void Awake() {
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable() {
        state = State.Ready;
        lastFireTime = 0;
    }

    // 발사 시도
    public void Fire() {
        if (state == State.Ready
            && Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    private void Shot() {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(fireTransform.position,
            fireTransform.forward, out hit, fireDistance))
        {

            LivingEntity attackTarget
                = hit.collider.GetComponent<LivingEntity>();

            if (attackTarget != null)
            {
                Debug.Log(attackTarget);
                attackTarget.OnDamage(damage, hit.point, hit.normal);
            }

            hitPosition = hit.point;
        }
        else
        {
            hitPosition = fireTransform.position +
                          fireTransform.forward * fireDistance;
        }
        StartCoroutine(ShotEffect(hitPosition));
    }

    private IEnumerator ShotEffect(Vector3 hitPosition) {
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);
        bulletLineRenderer.enabled = false;
    }

}