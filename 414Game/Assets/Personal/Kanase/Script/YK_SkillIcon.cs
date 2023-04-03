/**
 * @file YK_SkillIcon.cs
 * @brief スキルのアイコン処理
 * @author 吉田叶聖
 * @date 2023/03/16
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class YK_SkillIcon : YK_UI
{
   
    public Image SkillIcon;
   

    private void Start()
    {
        m_eUIType = UIType.SkillIcon;
        m_eFadeState = FadeState.FadeNone;
        GetSetPos = SkillIcon.GetComponent<RectTransform>().position;

    }

    void Update()
    {
        switch (m_eFadeState)
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
        SkillIcon.transform.DOScale(new Vector3(1f, 1f, 0f), 0f);
        // 1秒でテクスチャをフェードイン
        SkillIcon.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
        });
    }

    public override void UIFadeOUT()
    {
        // 1秒で後X,Y方向を0.5倍に変更
        SkillIcon.transform.DOScale(new Vector3(0.5f, 0.5f, 0f), 1f);
        // 1秒でテクスチャをフェードアウト
        SkillIcon.DOFade(0f, 1f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
        });
    }

}
