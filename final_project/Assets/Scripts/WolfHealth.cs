using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

public class WolfHealth : LivingEntity{
    public Slider wolf_healthSlider; // 체력을 표시할 UI 슬라이더

    protected override void OnEnable() {
        base.OnEnable();
        wolf_healthSlider.maxValue = startingHealth;
        wolf_healthSlider.value = health;
    }

    public override void OnDamage(float damage, Vector3 hitPoint,
        Vector3 hitDirection) {

        base.OnDamage(damage, hitPoint, hitDirection);
        Debug.Log("Wolf on Damage ");
        wolf_healthSlider.value = health;
    }

    // 사망 처리
    public override void Die() {
        base.Die();
        wolf_healthSlider.gameObject.SetActive(false);

        gameObject.SetActive(false);
        Destroy(gameObject);
    }

}