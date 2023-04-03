/**
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
    [SerializeField] private int m_nMaxHP = 100;
    [SerializeField] Slider HP;
    [SerializeField] private UnityEngine.UI.Image FrontFill;    //バーの表面のテクスチャ
    [SerializeField] private UnityEngine.UI.Image BackFill;     //後ろのバーの表面のテクスチャ
    [SerializeField] private UnityEngine.UI.Image BackGround;   //バーの裏のテクスチャ

    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.HPBar;
        m_eFadeState = FadeState.FadeNone;
        GetSetVisible = true;
        GetSetHP = m_nMaxHP;
        //UIが動くようならUpdateにかかなかん
        GetSetPos = HP.GetComponent<RectTransform>().position;
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

        switch(m_eFadeState)
        {
            case FadeState.FadeIN:
                UIFadeIN();
                break;
            case FadeState.FadeOUT:
                UIFadeOUT();
                break;
        }
    }

    public override void UIFadeIN()
    {
        // 1秒で後X,Y方向を元の大きさに変更
        HP.transform.DOScale(new Vector3(1.5f, 3f, 0f), 0f);
        // 1秒でテクスチャをフェードイン
        FrontFill.DOFade(1f, 0f);
        BackFill.DOFade(1f, 0f);
        BackGround.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
        });
    }

    public override void UIFadeOUT()
    {
        // 1秒で後X,Y方向を0.5倍に変更
        HP.transform.DOScale(new Vector3(0.5f, 0.5f, 0f), 1f);
        // 1秒でテクスチャをフェードアウト
        FrontFill.DOFade(0f, 1f);
        BackFill.DOFade(0f, 1f);
        BackGround.DOFade(0f, 1f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
        });
    }
}
