using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarControl : MonoBehaviour
{
    [SerializeField] GameObject hp_bar_prefab = null;
    public List<Transform> t_rabbits;
    public List<GameObject> hp_bar_list;

    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;

        GameObject[] o_rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
        for (int i = 0; i < o_rabbits.Length; i++)
        {
            t_rabbits.Add(o_rabbits[i].transform);
            GameObject hp_bar = Instantiate(hp_bar_prefab, o_rabbits[i].transform.position, Quaternion.identity, transform);
            hp_bar_list.Add(hp_bar);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < t_rabbits.Count; i++)
        {
            hp_bar_list[i].transform.position = camera.WorldToScreenPoint(t_rabbits[i].position + new Vector3(0, 1f, 0));
        }
    }
}