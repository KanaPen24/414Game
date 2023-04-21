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
    [SerializeField] private int m_fTimeLimit;
    [SerializeField] private Text timerText;
    [SerializeField] private YK_Clock Clock;
    private float m_fTime;

    void Update()
    {
        if (Clock.GetSetStopTime)
        {
            return;
        }
        //フレーム毎の経過時間をtime変数に追加
        m_fTime += Time.deltaTime;
        //time変数をint型にし制限時間から引いた数をint型のlimit変数に代入
        int remaining = m_fTimeLimit - (int)m_fTime;
        //timerTextを更新していく
        timerText.text = remaining.ToString("D3");

        //制限時間が0になったら
        if(remaining <=0)
        {
            //ゲームオーバー
            GameManager.instance.GetSetGameState = GameState.GameOver;
        }
    }
}