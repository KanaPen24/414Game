﻿/**
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
    public GameObject Second;
    [SerializeField] private Image Clock;
    [SerializeField] private Image Second_Image;
    [SerializeField] private YK_Hand m_Hand;
    [SerializeField] private YK_Time m_Time;
    [SerializeField] float timerLimit=5;
    float seconds = 0f;
    private int m_nTimeCount = 3;
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

        //変更　ClockのUpdateClock関数を呼び出す
        //　　　引数は_updateTimer()のtimerの値
       // UpdateClock(_updateTimer());
        Second.transform.eulerAngles = new Vector3(0, 0, ((float)dt.Second / 60 * -360 + (float)dt.Millisecond / 60 / 1000 * -360) * 10);
        if (m_bStopTime)
            //StopTimeFalseを5秒後に呼び出す
            Invoke(nameof(UIFadeIN), 5.0f);
    }
    public override void UIFadeIN()
    {
        m_bStopTime = false;
        m_nTimeCount--;
        m_eFadeState = FadeState.FadeIN;
        // 1秒でテクスチャをフェードイン
        Clock.DOFade(1f, 0f);
        Second_Image.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            m_Hand.HandPull();
            Debug.Log("FadeIN終了");
        });
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
        m_bStopTime = true;
    }

    //使用回数を回復する
    public void HealTimeCount(int Heal)
    {
        if (m_nTimeCount > 3) 
        m_nTimeCount += Heal;
    }

    //fillAmountの値を変更する関数（ClockTimerから呼ばれる）
    public void UpdateClock(float second)
    {
        //受け取ったfloat型の値を代入する
        Clock.fillAmount = second;
    }

    float _updateTimer()
    {
        seconds += Time.deltaTime;
        float timer = seconds / timerLimit;

        //追加　float型のtimerを返す
        return timer;
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
    /**
 * @fn
 * 表示非表示のgetter・setter
 * @return m_bTimeCount(int)
 * @brief 時止めフラグを返す・セット
 */
    public int GetSetTimeCount
    {
        get { return m_nTimeCount; }
        set { m_nTimeCount = value; }
    }
}