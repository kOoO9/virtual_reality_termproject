using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float speed = 5f; // 오브젝트 이동 속도

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 방향키 입력에 따라 이동 방향 설정
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movement.Normalize(); // 이동 방향을 정규화하여 동일한 속도로 이동

        // 이동 방향과 속도를 곱하여 이동 거리 계산
        Vector3 moveDistance = movement * speed * Time.deltaTime;

        // 현재 위치에 이동 거리를 더하여 새로운 위치 계산
        transform.position += moveDistance;
    }
}

