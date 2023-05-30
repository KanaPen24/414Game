using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ON_BlurController : MonoBehaviour
{
    [SerializeField] private ON_Blur[] ON_Blurs;
    [SerializeField] private float trailRate = 20.0f;       // 小さいほどブレる
    private float value = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < ON_Blurs.Length; ++i)
        {
            ON_Blurs[i].SetTrailRate(value);
        }
    }

    public void SetBlur(bool flg)
    {
        if (flg)
        {
            value = trailRate;
        }
        else
        {
            value = 100.0f;
        }
    }
}
