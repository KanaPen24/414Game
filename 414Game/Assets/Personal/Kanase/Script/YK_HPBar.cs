﻿/**
 * @file   YK_HPBar.cs
 * @brief  体力バー
 * @author 吉田叶聖
 * @date   2023/03/17
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
    [SerializeField] private Image BackGround;   //バーの裏のテクスチャ
    [SerializeField] private Image Frame;        //フレーム
    [SerializeField] private YK_Hand m_Hand;

    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.HPBar;
        m_eFadeState = FadeState.FadeNone;
        GetSetVisible = false;
        //UIが動くようならUpdateにかかなかん
        GetSetPos = HP.GetComponent<RectTransform>().anchoredPosition;
    }

    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 1秒で後X,Y方向を元の大きさに変更
        HP.transform.DOScale(new Vector3(1.5f, 3f, 0f), 0f);
        // 1秒でテクスチャをフェードイン
        FrontFill.DOFade(1f, 0f);
        BackFill.DOFade(1f, 0f);
        Frame.DOFade(1f, 0f);
        BackGround.DOFade(1f, 0f).OnComplete(() =>
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
        HP.transform.DOScale(new Vector3(0.5f, 0.5f, 0f), 1f);
        // 1秒でテクスチャをフェードアウト
        FrontFill.DOFade(0f, 1f);
        BackFill.DOFade(0f, 1f);
        Frame.DOFade(0f, 1f);
        BackGround.DOFade(0f, 1f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
    }
}
