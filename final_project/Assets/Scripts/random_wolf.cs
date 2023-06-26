using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class random_wolf : MonoBehaviour
{
    public GameObject rangeObject;
    BoxCollider rangeCollider;
    public GameObject wolf_rand;
    public float minSpawnTime = 10f; // 최소 생성 주기
    public float maxSpawnTime = 30f; // 최대 생성 주기

    private bool isdestroy = false;
    private GameObject instantwolf;
    
    private void Awake()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
    }
    
    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;
        
        range_X = Random.Range( (range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range( (range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
    
    // 소환할 Object
    private void Start()
    {
        StartCoroutine(RandomRespawn_Coroutine());
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        while (true)
        {
            if(instantwolf == null && isdestroy == true){
                isdestroy = false;
                wolfattack.isfence = false;
                wolfattack.isfox = false;
                wolfattack.isfoxfox = false;
                wolfattack.isfencestop = false;
            }
            if(isdestroy == false){
                float randomSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(randomSpawnTime);
                // 생성 위치 부분에 위에서 만든 함수 Return_RandomPosition() 함수 대입

                instantwolf = Instantiate(wolf_rand, Return_RandomPosition(), Quaternion.identity);
                isdestroy = true;
            }
            yield return null;
        }
    }
}
