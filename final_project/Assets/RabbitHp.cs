using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RabbitHp : MonoBehaviour
{
    public float startingHealth = 100f;
    public float healthDecreaseRate = 1f;
    public float healthIncreaseAmount = 50f;
    public GameObject carrotPrefab;

    public Slider healthBar;
    private float currentHealth;
    public bool isDead = false;

    private Transform mainCameraTransform;

    void Start()
    {
        mainCameraTransform = Camera.main.transform;
        // Rabbit 오브젝트마다 상태바 생성
        healthBar.gameObject.SetActive(true);
        healthBar.maxValue = startingHealth;
        healthBar.value = startingHealth;
        currentHealth = startingHealth;
    }

    void Update()
    {
        // 체력 감소
        currentHealth -= healthDecreaseRate * Time.deltaTime;
        currentHealth = Mathf.Max(currentHealth, 0f);
        healthBar.value = currentHealth;

        // 체력이 0이 되면 Dead 애니메이션 실행
        if (currentHealth <= 0f)
        {
            isDead = true;
            healthBar.gameObject.SetActive(false);
            wolfattack.isfox = true;

        }
        healthBar.transform.LookAt(mainCameraTransform);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Carrot"))
        {
            // 먹이를 먹고 체력 충전
            currentHealth += healthIncreaseAmount;
            currentHealth = Mathf.Min(currentHealth, startingHealth);
            healthBar.value = currentHealth;

            // 먹이 오브젝트 삭제
            Destroy(collision.collider.gameObject);
            GameObject carrotObject = Instantiate(carrotPrefab, new Vector3(1.5f, 0.75f, 6f), Quaternion.identity);
        }

        if (collision.collider.CompareTag("enemy"))
        {
            Debug.Log("wolf attack");
            currentHealth -= healthIncreaseAmount;
            currentHealth = Mathf.Min(currentHealth, startingHealth);
            healthBar.value = currentHealth;
        }
    }

}
