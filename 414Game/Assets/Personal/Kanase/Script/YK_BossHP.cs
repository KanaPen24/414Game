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
    [SerializeField] private Slider BossSlider;     // シーンに配置したSlider格納用
    [SerializeField] private GameObject BossBar;    // ボスのバーの大元
    [SerializeField] private NK_BossSlime Boss;

    // Use this for initialization
    void Start()
    {
        m_eUIType = UIType.BossBar;
        m_eFadeState = FadeState.FadeNone;
        GetSetVisible = false;
        BossSlider.maxValue = Boss.GetSetMaxHp;// Sliderの最大値を敵キャラのHP最大値と合わせる
        BossSlider.value = Boss.GetSetHp;   // Sliderの初期状態を設定（HP満タン）
    }

    // Update is called once per frame
    void Update()
    {
        // Sliderの更新
        BossSlider.value = Boss.GetSetHp;

        // Sliderが最小値になったら（例：ボスキャラを倒したら）
        if (BossSlider.value <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
