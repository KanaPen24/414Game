/**
 * @file YK_Watch.cs
 * @brief 時計のモデル処理
 * @author 吉田叶聖
 * @date   2023/05/25
 *         初版作成日
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YK_Watch : MonoBehaviour
{
    [SerializeField] private Image Clock_Inner;     //時計の赤い部分
    [SerializeField] private YK_Time time;
    private int m_nTimeLimit;
    private void Start()
    {
        //タイムリミット取得
        m_nTimeLimit = time.GetSetTimeLimit;
    }
    // Update is called once per frame
    void Update()
    {
        // ゲームがプレイ中以外は更新しない
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)
            return;
        //受け取ったfloat型の値を代入する
        Clock_Inner.fillAmount = 1.0f - time.GetSetNowTime / (float)m_nTimeLimit;
    }
}
