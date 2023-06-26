using UnityEngine;

public class BagController : MonoBehaviour
{
    public delegate void LandedAction();
    public event LandedAction OnLanded;

    void OnCollisionEnter(Collision collision)
    {
        // plane에 충돌했을 때 호출되는 콜백 함수
        if (collision.gameObject.CompareTag("Plane"))
        {
            // bag이 plane에 안착했음을 알리는 이벤트 호출
            OnLanded?.Invoke();
        }
    }
}
