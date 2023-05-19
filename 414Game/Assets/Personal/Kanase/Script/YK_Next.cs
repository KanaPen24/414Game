/**
 * @file YK_Next.cs
 * @brief NextUIの処理
 * @author 吉田叶聖
 * @date 2023/03/25
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class YK_Next : YK_UI
{
    [SerializeField] private Image Next;
    [SerializeField] private Vector3 m_MinScale = new Vector3(0.5f, 0.5f, 0.5f); // 最小サイズ
    [SerializeField] private float m_fDelTime = 0.5f; // 減算していく時間
    private bool m_bVisibleNext;
    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.Next; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        //UIが動くようならUpdateにかかなかん
        GetSetPos = Next.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = Next.transform.localScale;
        UIFadeOUT();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //NextUIを表示
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 1秒で後X,Y方向を元の大きさに変更
        Next.transform.DOScale(GetSetScale, 0f);
        // 1秒でテクスチャをフェードイン
        Next.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;     
        });
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // 1秒で後X,Y方向を0.5倍に変更
        Next.transform.DOScale(m_MinScale, m_fDelTime);
        // 1秒でテクスチャをフェードアウト
        Next.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
        });
    }
    /**
 * @fn
 * 表示非表示のgetter・setter
 * @return m_bVisibleNext(bool)
 * @brief 表示非表示処理
 */
    public bool GetSetVisibleFlg
    {
        get { return m_bVisibleNext; }
        set { m_bVisibleNext = value; }
    }
}