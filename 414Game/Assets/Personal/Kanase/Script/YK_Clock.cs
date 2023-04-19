/**
 * @file YK_Clock.cs
 * @brief アナログ時計の処理
 * @author 吉田叶聖
 * @date 2023/04/17
 */
using System;   // DateTimeに必要
using System.Collections;
using UnityEngine;

public class YK_Clock : MonoBehaviour
{

    public bool m_bSecTick;   // 秒針を秒ごとに動かすか
    public GameObject Second;

    void Start()
    {
    }

    void Update()
    {
        DateTime dt = DateTime.Now;
        if (m_bSecTick)
            Second.transform.eulerAngles = new Vector3(0, 0, ((float)dt.Second / 60 * -360));
        else
            Second.transform.eulerAngles = new Vector3(0, 0, ((float)dt.Second / 60 * -360 + (float)dt.Millisecond / 60 / 1000 * -360) * 10);
        

    }
}