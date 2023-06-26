using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitManager : MonoBehaviour
{
    public GameObject bagPrefab;
    public GameObject rabbitPrefab;
    public Transform plane;

    private GameObject bagObject;

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            // 스페이스바를 눌렀을 때 오브젝트 생성
            CreateBag();
        }*/
    }

    public void CreateBag()
    {
        // bag 오브젝트 생성
        bagObject = Instantiate(bagPrefab, new Vector3(0, 10f, 0), Quaternion.identity);

        // bag 오브젝트에 Rigidbody 컴포넌트 추가
        Rigidbody bagRigidbody = bagObject.AddComponent<Rigidbody>();

        // Rigidbody 속성 설정
        bagRigidbody.isKinematic = false;
        bagRigidbody.useGravity = true;

        // bag 오브젝트가 plane에 안착할 때 호출되는 콜백 함수 등록
        bagRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        bagRigidbody.GetComponent<BagController>().OnLanded += SpawnRabbit;

        // bag 오브젝트를 1초 뒤에 제거하는 코루틴 실행
        Destroy(bagObject, 2.5f);
    }

    void SpawnRabbit()
    {
        // bag 오브젝트의 위치에 rabbit 오브젝트 생성
        GameObject rab = Instantiate(rabbitPrefab, bagObject.transform.position, Quaternion.identity);
        rab.transform.SetParent(transform);
    }
}
