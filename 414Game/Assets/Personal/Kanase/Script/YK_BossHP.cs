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
using DG.Tweening;

public class YK_BossHP : YK_UI
{
    [SerializeField] private Slider BossSlider;     // シーンに配置したSlider格納用
    [SerializeField] private GameObject BossBar;    // ボスのバーの大元
    [SerializeField] private NK_BossSlime Boss;
    [SerializeField] private Image FrontFill;    //バーの表面のテクスチャ
    [SerializeField] private Image BackFill;     //後ろのバーの表面のテクスチャ
    [SerializeField] private Image BackGround;   //バーの裏のテクスチャ
    [SerializeField] private Image Frame;        //フレーム
    [SerializeField] private YK_Hand m_Hand;
    [SerializeField] private Vector3 m_MinScale = new Vector3(0.5f, 0.5f, 0.0f); // 最小サイズ
    [SerializeField] private float m_fDelTime = 0.5f; // 減算していく時間

    // Use this for initialization
    void Start()
    {
        m_eUIType = UIType.BossBar; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;  
        BossSlider.maxValue = Boss.GetSetMaxHp;// Sliderの最大値を敵キャラのHP最大値と合わせる
        BossSlider.value = Boss.GetSetHp;   // Sliderの初期状態を設定（HP満タン）

        //座標取得
        GetSetPos = BossSlider.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = BossSlider.transform.localScale;
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

    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 1秒で後X,Y方向を元の大きさに変更
        BossSlider.transform.DOScale(GetSetScale, 0f);
        // 1秒でテクスチャをフェードイン
        FrontFill.DOFade(1f, 0f);
        Frame.DOFade(1f, 0f);
        BackFill.DOFade(1f, 0f);
        BackGround.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;            
            Debug.Log("FadeIN終了");
        });
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // 1秒で後X,Y方向を0.5倍に変更
        BossSlider.transform.DOScale(m_MinScale, m_fDelTime);
        // 1秒でテクスチャをフェードアウト
        FrontFill.DOFade(0f, m_fDelTime);
        Frame.DOFade(0f, m_fDelTime);
        BackFill.DOFade(0f, m_fDelTime);
        BackGround.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
    }
}
