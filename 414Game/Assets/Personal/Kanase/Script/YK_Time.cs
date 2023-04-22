/**
 * @file YK_Time.cs
 * @brief テキスト版時計
 * @author 吉田叶聖
 * @date 2023/04/17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YK_Time : MonoBehaviour
{
    [SerializeField] private int m_nTimeLimit = 200;    //タイムリミット
    [SerializeField] private Text timerText;            //表示するテキスト
    [SerializeField] private YK_Clock Clock;
    private float m_fTime;
    private int  m_nNowTime;       //現在時間

    void Update()
    {
        if (Clock.GetSetStopTime)
        {
            return;
        }
        //フレーム毎の経過時間をtime変数に追加
        m_fTime += Time.deltaTime;
        //time変数をint型にし制限時間から引いた数をint型のlimit変数に代入
        m_nNowTime = m_nTimeLimit - (int)m_fTime;

        if (Clock.GetSetTimeCount <= 2)
            m_nNowTime %= 100;  //3桁目を減らす
        if(Clock.GetSetTimeCount <= 1)
            m_nNowTime %= 10;   //2桁目を減らす
        if (Clock.GetSetTimeCount <= 0)
            m_nNowTime %= 1;    //1桁目を減らす

        //timerTextを更新していく
        timerText.text = m_nNowTime.ToString("D3");

        //制限時間が0になったら
        if(m_nNowTime <=0)
        {
            //ゲームオーバー
            GameManager.instance.GetSetGameState = GameState.GameOver;
        }
    }

    //時間を増やす関数
    void AddTime(int Time)
    {
        m_nTimeLimit += Time;
    }

    /**
  * @fn
  * 表示非表示のgetter・setter
  * @return m_nTimeLimit(int)
  * @brief 制限時間を返す・セット
  */
    public int GetSetNowTime
    {
        get { return m_nNowTime; }
        set { m_nNowTime = value; }
    }
}