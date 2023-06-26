using UnityEngine;

public class PlayerShooter : LivingEntity {
    public Gun gun; // 사용할 총
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점
    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    private Animator playerAnimator; // 애니메이터 컴포넌트

    private void Start() {
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable() {
        gun.gameObject.SetActive(true);
    }

    private void OnDisable() {
        gun.gameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼 클릭
        {
            playerAnimator.SetTrigger("Gun");
            gun.Fire();
            muzzleFlashEffect.Play();
        }
        else if(Input.GetMouseButtonUp(1)) // 마우스 오른쪽 버튼 클릭
        {
            playerAnimator.SetTrigger("movement");
        }
    }

    private void OnAnimatorIK(int layerIndex) {
        gunPivot.position =
            playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand,
            leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand,
            leftHandMount.rotation);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand,
            rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand,
            rightHandMount.rotation);
    }
}