/**
 * @file   YK_TimeLImit.cs
 * @brief  タイムリミット
 * @author 吉田叶聖
 * @date   2023/05/05
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YK_TimeLimit : MonoBehaviour
{
    [SerializeField] private Image Clock;
    [SerializeField] private YK_Time time;
    // Start is called before the first frame update
    void Start()
    {
        Clock.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //受け取ったfloat型の値を代入する
        Clock.fillAmount = 1.0f - time.GetSetNowTime / 200.0f;
    }
}
