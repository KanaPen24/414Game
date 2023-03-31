/**
 * @file YK_BossHP.cs
 * @brief ボスの体力
 * @author 吉田叶聖
 * @date 2023/03/28
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YK_BossHP : YK_UI
{
    private const int m_nMaxHp = 100;               // 敵キャラのHP最大値を100とする
    private int m_nBossHP;                          // 現在のHP
    public Slider BossSlider;                       // シーンに配置したSlider格納用
    [SerializeField] private GameObject BossBar;    // ボスのバーの大元
    [SerializeField] private GameObject Boss;       // 登場するボスのオブジェクト

    // Use this for initialization
    void Start()
    {
        m_eUIType = UIType.BossBar;
        GetSetVisible = false;
        GetSetHP = m_nMaxHp;
        BossSlider.maxValue = m_nMaxHp;    // Sliderの最大値を敵キャラのHP最大値と合わせる
        m_nBossHP = m_nMaxHp;      // 初期状態はHP満タン
        BossSlider.value = m_nBossHP;   // Sliderの初期状態を設定（HP満タン）
        BossBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            DelLife(10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            AddLife(10,m_nMaxHp);
            VisibleTrue();
        }
        m_nBossHP = GetSetHP;
        BossSlider.value = m_nBossHP;   // Sliderを更新
        // Sliderが最小値になったら（例：ボスキャラを倒したら）
        if (BossSlider.value <= 0)
        {
            Destroy(Boss);      // ボスを消去
            Destroy(BossSlider);   // Sliderも消去
        }
    }
    //ボスバー表示
    public void VisibleTrue()
    {
        BossBar.gameObject.SetActive(true);
    }
}
