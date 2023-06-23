﻿/**
 * @file YK_PlayerHP.cs
 * @brief プレイヤーの体力
 * @author 吉田叶聖
 * @date 2023/03/06
 * @Update 2023/04/20 HPSliderをPlayer参照に変更(Ihara)
 * @Update 2023/04/20 HP減少処理を改良(Ihara)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YK_PlayerHP : MonoBehaviour
{
    //　HPを一度減らしてからの経過時間
    private float m_fCountLime;
    //  HPが遅れて減る時間
    [SerializeField] private float m_fDelCount;

    //　HP表示用スライダー
    [SerializeField] private Slider HpSlider;

    //　一括HP表示用スライダー
    [SerializeField] private Slider BulkHPSlider;

    // Player
    [SerializeField] private IS_PlayerParam PlayerParam;

    // HP減少フラグ
    private bool m_bDelFlg;

    void Start()
    {
        // メンバの初期化
        HpSlider.maxValue = PlayerParam.m_nMaxHP;
        HpSlider.value = PlayerParam.m_nHP;
        BulkHPSlider.maxValue = PlayerParam.m_nMaxHP;
        BulkHPSlider.value = PlayerParam.m_nHP;
        m_bDelFlg = false;
    }

    void Update()
    {
        if(BulkHPSlider.value > PlayerParam.m_nHP)
        {
            DelLife((int)HpSlider.value - PlayerParam.m_nHP);
        }
        else if(BulkHPSlider.value < PlayerParam.m_nHP)
        {
            AddLife(PlayerParam.m_nHP - (int)HpSlider.value);
        }

        if(m_bDelFlg)
        {
            if(m_fCountLime >= m_fDelCount)
            {
                HpSlider.value = PlayerParam.m_nHP;
                m_fCountLime = 0.0f;
                m_bDelFlg = false;
            }
            else m_fCountLime += Time.deltaTime;
        }
    }

    //　ダメージ値を追加するメソッド
    public void DelLife(int damage)
    {
        m_fCountLime = 0.0f;
        BulkHPSlider.value = PlayerParam.m_nHP;
        m_bDelFlg = true;
    }
    // ダメージ処理
    // 回復処理
    public void AddLife(int damege)
    {
        HpSlider.value = PlayerParam.m_nHP;
        BulkHPSlider.value = PlayerParam.m_nHP;
    }
}
