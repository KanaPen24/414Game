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
    [SerializeField] private YK_Hand m_Hand;
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
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UIFadeOUT();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UIFadeIN();
        }
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
            m_Hand.HandPull();
            Debug.Log("FadeIN終了");
        });
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // 1秒で後X,Y方向を0.5倍に変更
        Next.transform.DOScale(new Vector3(0.5f, 0.5f, 0f), 1f);
        // 1秒でテクスチャをフェードアウト
        Next.DOFade(0f, 1f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
    }
}
