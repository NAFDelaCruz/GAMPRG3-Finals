using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarScript : MonoBehaviour
{
    public float fill_perc;

    public EntityStats entityStats;

    public GameObject Fill;

    // Start is called before the first frame update
    void Start()
    {
        entityStats = this.GetComponentInParent<EntityStats>();
    }

    // Update is called once per frame
    void Update()
    {
        fill_perc = (float)entityStats.Curr_HP / (float)entityStats.HP;
        Fill.GetComponent<Image>().fillAmount = fill_perc;
    }
}
