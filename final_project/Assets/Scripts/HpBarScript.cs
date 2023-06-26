using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarScript : MonoBehaviour
{
    public Slider hpbar;
    public float maxHp;
    public float currenthp;

    void Update()
    {
        transform.position +=  new Vector3(0, 0, 0);
        hpbar.value = currenthp / maxHp;
    }
}