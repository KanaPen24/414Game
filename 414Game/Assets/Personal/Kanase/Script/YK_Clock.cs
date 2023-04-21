/**
 * @file YK_Clock.cs
 * @brief アナログ時計の処理
 * @author 吉田叶聖
 * @date 2023/04/17
 */
using System;   // DateTimeに必要
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class YK_Clock : YK_UI
{
    public bool m_bSecTick;   // 秒針を秒ごとに動かすか
    public GameObject Second;
    [SerializeField] private Image Clock;
    [SerializeField] private Image Second_Image;
    [SerializeField] private YK_Hand m_Hand;
    private bool m_bStopTime = false;   //時止め時間かどうか

    void Start()
    {
        m_eUIType = UIType. Clock; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        //UIが動くようならUpdateにかかなかん
        GetSetPos = Clock.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = Clock.transform.localScale;
    }


    void Update()
    {
        DateTime dt = DateTime.Now;
        if (m_bSecTick)
            Second.transform.eulerAngles = new Vector3(0, 0, ((float)dt.Second / 60 * -360));
        else
            Second.transform.eulerAngles = new Vector3(0, 0, ((float)dt.Second / 60 * -360 + (float)dt.Millisecond / 60 / 1000 * -360) * 10);
    }
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 1秒でテクスチャをフェードイン
        Clock.DOFade(1f, 0f);
        Second_Image.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            m_Hand.HandPull();
            Debug.Log("FadeIN終了");
        });
        m_bStopTime = false;
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // 1秒でテクスチャをフェードアウト
        Clock.DOFade(0f, 1f);
        Second_Image.DOFade(0f, 1f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
    }

    /**
   * @fn
   * 表示非表示のgetter・setter
   * @return m_bStopTime(bool)
   * @brief 時止めフラグを返す・セット
   */
    public bool GetSetStopTime
    {
        get { return m_bStopTime; }
        set { m_bStopTime = value; }
    }
}