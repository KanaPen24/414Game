/**
 * @file YK_Time.cs
 * @brief テキスト版時計
 * @author 吉田叶聖
 * @date 2023/04/17
 * @Update 2023/05/21 ゲームオーバー処理編集(Ihara)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YK_Time : MonoBehaviour
{
    [SerializeField] private int m_nTimeLimit;          //タイムリミット
    private int m_nTimeLimitStorage;                    //タイムリミット保存用
    [SerializeField] private Text timerText;            //表示するテキスト
    [SerializeField] private YK_Clock Clock;            //時止め使うためのコンポーネント
    [SerializeField] private IS_Player Player;          //プレイヤーをアタッチ
    [SerializeField] ON_VolumeManager PostEffect;       //ポストエフェクト
    private Outline outline;
    [SerializeField] private float m_fTime;              //進行時間
    [SerializeField] private int m_nNearLimit;           //限界時間が近くなったら
    private float m_fPostEffect_Time;   //ポストエフェクト用の時間
    private float m_rate = 0.0f;        //ポストエフェクト用の割合
    private bool m_bPostEffect = false; //ポストエフェクト用のフラグ
    private bool m_bTimer = true;       //タイマー用のフラグ
    private bool m_bOnce = false;       //一回だけ使うフラグ
    [SerializeField] private int m_nNowTime;    //現在時間
    [SerializeField] private ParticleSystem Effect;    //回復エフェクト
    [SerializeField] private Material TextMaterial;    //ラスタースクロール
    private YK_ScaleDownOrUp ScaleDownUp;

    private void Start()
    {
        outline = this.GetComponent<Outline>();
        m_nNowTime = m_nTimeLimit;
        ScaleDownUp = this.GetComponent<YK_ScaleDownOrUp>();
        m_nTimeLimitStorage = m_nTimeLimit;
    }

    void Update()
    {

        //ポストエフェクトの時間をリセットしておく
        if (m_bTimer)
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
            //マテリアル変更
            timerText.material = TextMaterial;
            return;
        }
        else if (m_bOnce)
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
            //マテリアル変更
            timerText.material = null;
            //これをすることで最初の起動時に流れないようになる
            if (m_rate <= 0.0f)
                m_bOnce = false;
        }

        // ゲームがプレイ中以外は更新しない
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)
            return;

        //フレーム毎の経過時間をtime変数に追加
        m_fTime += Time.deltaTime;
        m_fTime = Mathf.Max(m_fTime, 0.0f);
        //time変数をint型にし制限時間から引いた数をint型のlimit変数に代入
        m_nNowTime = m_nTimeLimit - (int)m_fTime;

        switch(Clock.GetSetTimeCount)
        {
            case 3:
                m_nTimeLimit = m_nTimeLimitStorage;
                break;
            case 2:
                m_nTimeLimit = 99;
                break;
            case 1:
                m_nTimeLimit = 9;
                m_fTime %= 10;
                Clock.GetSetTimeCount = 0;
                break;
            case -1:
                m_nNowTime %= 1;    //1桁目を減らす
                break;
        }           
                       
        //timerTextを更新していく
        timerText.text = m_nNowTime + "";

        if (m_nNowTime <= m_nNearLimit)
        {
            ScaleDownUp.GetSetScalelFlg = true;
            timerText.color = Color.red;
            
        }

        //制限時間が0になったら
        if (m_nNowTime <=0)
        {
            //タイムアップかえる
            YK_GameOver.instance.GetSetGameOverState = GameOverState.TimeLimit;
            //ゲームオーバー
            Player.GetSetPlayerState = PlayerState.PlayerGameOver;
            GameManager.instance.GetSetGameState = GameState.GameOver;
        }
    }

    //時間を増やす関数
    public void AddTime(int Time)
    {
        //経過時間を引くことで現在時間が足される
        m_fTime -= Time;
        EffectPlay();
    }

    public void EffectPlay()
    {
        Effect.Play();
    }

    /**
  * @fn
  * 時間のgetter・setter
  * @return m_nNowTime(int)
  * @brief 現在時間を返す・セット
  */
    public int GetSetNowTime
    {
        get { return m_nNowTime; }
        set { m_nNowTime = value; }
    }
    /**
* @fn
* タイマー用フラグのgetter・setter
* @return m_bTimer(bool)
* @brief タイマー用フラグを返す・セット
*/
    public bool GetSetTimeFlg
    {
        get { return m_bTimer; }
        set { m_bTimer = value; }
    }
    /**
* @fn
* 時間のgetter・setter
* @return m_nTimeLimit(int)
* @brief 制限時間を返す・セット
*/
    public int GetSetTimeLimit
    {
        get { return m_nTimeLimit; }
        set { m_nTimeLimit = value; }
    }
}