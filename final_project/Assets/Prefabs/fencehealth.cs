using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class fencehealth : LivingEntity {
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더
    public ParticleSystem hitEffect; // 피격시 재생할 파티클 효과

    protected override void OnEnable() {
        base.OnEnable();
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;
    }

    public override void OnDamage(float damage, Vector3 hitPoint,
        Vector3 hitDirection) {
        if (!dead)
        {

            hitEffect.Play();
        }

        base.OnDamage(damage, hitPoint, hitDirection);
        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die() {
        base.Die();
        healthSlider.gameObject.SetActive(false);

        gameObject.SetActive(false);
        Destroy(gameObject);

        wolfattack.isfencestop = true;
        Debug.Log(wolfattack.isfencestop); //fence 부심
    }

}