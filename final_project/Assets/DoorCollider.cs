using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그가 "Player"인 경우에는 Collider 효과를 무시한다
        if (collision.collider.CompareTag("Player"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<BoxCollider>());
        }
    }
}
