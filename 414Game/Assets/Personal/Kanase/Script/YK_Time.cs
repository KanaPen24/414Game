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
    [SerializeField] private YK_Clock Clock;            //時止め使うためのコンポーネント
    [SerializeField] ON_VolumeManager PostEffect;       //ポストエフェクト
    private Outline outline;
    private float m_fTime;              //進行時間
    private float m_fPostEffect_Time;   //ポストエフェクト用の時間
    private float m_rate = 0.0f;        //ポストエフェクト用の割合
    private bool m_bPostEffect = false; //ポストエフェクト用のフラグ
    private bool m_bTimer = true;       //タイマー用のフラグ
    private bool m_bOnce = false;       //一回だけ使うフラグ
    private int  m_nNowTime;            //現在時間

    private void Start()
    {
        outline = this.GetComponent<Outline>();
    }

    void Update()
    {
        // ゲームがプレイ中以外は更新しない
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)
            return;

        //時間をリセットしておく
        if(m_bTimer)
        {
            m_fPostEffect_Time = 0.0f;
        }

        //時止め中
        if (Clock.GetSetStopTime)
        {
            m_bTimer = false;
            //時止めのポストエフェクトを減らしていく処理
            m_fPostEffect_Time += Time.deltaTime;
            m_rate = Mathf.Lerp(0.0f, 1.0f, m_fPostEffect_Time);
            //ポストエフェクトの変更
            PostEffect.ChangeTimePostEffect(m_rate);
            if (m_rate >= 1.0f)
                m_bOnce = true;
            //テキストカラー変更
            timerText.color = Color.black;
            outline.effectColor = Color.white;
            return;
        }
        else if(m_bOnce)
        {
            m_bTimer = false;
            //時止めのポストエフェクトを減らしていく処理
            m_fPostEffect_Time += Time.deltaTime;
            m_rate = Mathf.Lerp(1.0f, 0.0f, m_fPostEffect_Time);
            //ポストエフェクトの変更
            PostEffect.ChangeTimePostEffect(m_rate);
            //テキストカラー変更
            timerText.color = Color.white;
            outline.effectColor = Color.black;
            //これをすることで最初の起動時に流れないようになる
            if (m_rate <= 0.0f)
                m_bOnce = false;
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
    public void AddTime(int Time)
    {
        m_nTimeLimit += Time;
        
    }

    /**
  * @fn
  * 時間のgetter・setter
  * @return m_nNowTime(int)
  * @brief 制限時間を返す・セット
  */
    public int GetSetNowTime
    {
        get { return m_nNowTime; }
        set { m_nNowTime = value; }
    }
    /**
* @fn
* 表示非表示のgetter・setter
* @return m_bTimer(int)
* @brief 制限時間を返す・セット
*/
    public bool GetSetTimeFlg
    {
        get { return m_bTimer; }
        set { m_bTimer = value; }
    }

}