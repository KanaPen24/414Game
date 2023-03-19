/**
 * @file   YK_HPBar.cs
 * @brief  体力バー
 * @author 吉田叶聖
 * @date   2023/03/17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class YK_HPBar : YK_UI
{
    [SerializeField] private int m_nMaxHP = 100;
    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.HPBar;
        GetSetVisible = true;
        GetSetHP = m_nMaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetSetHP <= 50)
        {
            //  武器の見た目変更処理
        }
        if (GetSetHP <= 25)
        {
            //  武器の見た目変更処理
        }
        if (GetSetHP <= 0)
        {
            //  ゲームオーバー処理
        }
    }
}
