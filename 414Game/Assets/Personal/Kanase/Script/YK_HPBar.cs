/**
 * @file   YK_HPBar.cs
 * @brief  体力バー
 * @author 吉田叶聖
 * @date   2023/03/17
 * @date   2023/04/21   画像追加に伴う処理の追加
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class YK_HPBar : YK_UI
{
    [SerializeField] Slider HP;
    [SerializeField] private Image FrontFill;    //バーの表面のテクスチャ
    [SerializeField] private Image BackFill;     //後ろのバーの表面のテクスチャ
    [SerializeField] private Image Frame;        //フレーム
    [SerializeField] private Image Crack;        //ヒビの画像
    [SerializeField] private Image Refraction;   //反射光
    [SerializeField] private Image OutLine;      //アウトライン
    [SerializeField] private YK_Hand m_Hand;
    [SerializeField] private Vector3 m_MinScale = new Vector3(0.5f, 0.5f, 0.0f); // 最小サイズ
    [SerializeField] private float m_fDelTime = 0.5f; // 減算していく時間

    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.HPBar;   //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        GetSetVisible = false;
        OutLine.enabled = false;
        //UIが動くようならUpdateにかかなかん
        GetSetPos = HP.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = HP.transform.localScale;
    }


    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 1秒で後X,Y方向を元の大きさに変更
        HP.transform.DOScale(GetSetScale, 0f);
        // 1秒でテクスチャをフェードイン
        FrontFill.DOFade(1f, 0f);
        BackFill.DOFade(1f, 0f);
        Crack.DOFade(1f, 0f);
        Refraction.DOFade(1f, 0f);
        OutLine.DOFade(1f, 0f);
        Frame.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            m_Hand.HandPull();
            Debug.Log("FadeIN終了");
        });
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // 1秒で後X,Y方向を0.5倍に変更
        HP.transform.DOScale(m_MinScale, m_fDelTime);
        // 1秒でテクスチャをフェードアウト
        FrontFill.DOFade(0f, m_fDelTime);
        BackFill.DOFade(0f, m_fDelTime);
        Crack.DOFade(0f, m_fDelTime);
        Refraction.DOFade(0f, m_fDelTime);
        OutLine.DOFade(0f, m_fDelTime);
        Frame.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Cursol")
        {
            OutLine.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
            OutLine.enabled = false;   
    }
}
