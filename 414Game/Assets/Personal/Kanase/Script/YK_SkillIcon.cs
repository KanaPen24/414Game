/**
 * @file YK_SkillIcon.cs
 * @brief スキルのアイコン処理
 * @author 吉田叶聖
 * @date 2023/03/16
 * @Update 2023/04/03 フェード処理実装(Ihara)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class YK_SkillIcon : YK_UI
{
    public Image SkillIcon;
    [SerializeField] private int m_nStuck; // 弾数ストック

    private void Start()
    {
        m_eUIType = UIType.SkillIcon;   //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;

        //座標取得
        GetSetPos = SkillIcon.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = SkillIcon.transform.localScale;

    }

    public override void UIFadeIN()
    {
        // 1秒で後X,Y方向を元の大きさに変更
        SkillIcon.transform.DOScale(GetSetScale, 0f);
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
