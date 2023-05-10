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
    public GameObject Second;
    [SerializeField] private Image Clock;           //時計本体
    [SerializeField] private Image Clock_Inner;     //時計の赤い部分
    [SerializeField] private Image Second_Image;    //時計の針
    [SerializeField] private Image OutLine;         //アウトライン
    [SerializeField] private YK_Hand m_Hand;
    [SerializeField] private YK_Time m_Time;
    [SerializeField] private IS_Player Player;
    [SerializeField] float timerLimit=5;
    [SerializeField] private Vector3 m_MinScale = new Vector3(0.5f, 0.5f, 0.0f); // 最小サイズ
    [SerializeField] private float m_fDelTime = 0.3f; // 減算していく時間
    [SerializeField] YK_Time Time; // 時間
    [SerializeField] ON_TimePostEffect PostEffect; // 時間
    private Vector3 Second_Scale;
    float seconds = 0f;
    private int m_nTimeCount = 3;
    private bool m_bStopTime = false;   //時止め時間かどうか
    private bool m_bOnce = true;

    void Start()
    {
        m_eUIType = UIType. Clock; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        OutLine.enabled = false;
        //UIが動くようならUpdateにかかなかん
        GetSetPos = Clock.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = Clock.transform.localScale;
        Second_Scale = Second.transform.localScale;
       
    }


    void Update()
    {        
        Second.transform.eulerAngles = new Vector3(0, 0, (Time.GetSetNowTime/200.0f)*360.0f);
        if (m_bStopTime && m_bOnce)
        {
            m_bOnce = false;
            PostEffect.ChangeTimePostEffect(true);
            Invoke(nameof(StopTimeSE), 3.0f);
            //StopTimeReleaseを5秒後に呼び出す
            Invoke(nameof(StopTimeRelease), 5.0f);
        }
    }

    public void StopTimeSE()
    {
        // SE再生
        IS_AudioManager.instance.PlaySE(SEType.SE_StopTime_Return);
    }

    public void StopTimeRelease()
    {
        PostEffect.ChangeTimePostEffect(false);
        // SE停止
        IS_AudioManager.instance.StopSE(SEType.SE_StopTime);
        m_bStopTime = false;
        m_bOnce = true;
        m_nTimeCount--;
        UIFadeIN();
        // BGM再生
        IS_AudioManager.instance.GetBGM(BGMType.BGM_Game).UnPause();
        Player.RemovedWeapon();
        Debug.Log("元戻る");
    }
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 1秒で後X,Y方向を元の大きさに変更
        this.gameObject.transform.DOScale(GetSetScale, 0f);
        Second.transform.DOScale(Second_Scale, 0f);
        // 1秒でテクスチャをフェードイン
        Clock.DOFade(1f, 0f);
        Clock_Inner.DOFade(1f, 0f);
        OutLine.DOFade(1f, 0f);
        Second_Image.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeIN終了");
        });
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // 1秒で後X,Y方向を0.5倍に変更
        this.gameObject.transform.DOScale(m_MinScale, m_fDelTime);
        Second.transform.DOScale(Second_Scale-m_MinScale, m_fDelTime);
        // 1秒でテクスチャをフェードアウト
        Clock.DOFade(0f, m_fDelTime);
        Clock_Inner.DOFade(0f, m_fDelTime);
        OutLine.DOFade(0f, m_fDelTime);
        Second_Image.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
    }

    //使用回数を回復する
    public void HealTimeCount(int Heal)
    {
        if (m_nTimeCount > 3) 
        m_nTimeCount += Heal;
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