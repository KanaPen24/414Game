/**
 * @file YK_Clock.cs
 * @brief アナログ時計の処理
 *        このファイルはアナログ時計の動作を制御するスクリプト。
 *        時計の針の回転や停止時間の管理、UIの表示・非表示などの機能を提供
 * @author 吉田叶聖
 *         このスクリプトの作成者
 * @date   2023/04/17
 *         初版作成日
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
    [SerializeField] private YK_Time m_Time;    
    [SerializeField] ON_VolumeManager PostEffect; // ポストエフェクト
    private Vector3 Second_Scale;
    float seconds = 0f;
    [SerializeField] private int m_nTimeCount = 3;
    [SerializeField] private bool m_bStopTime = false;   //時止め時間かどうか
    [SerializeField] private int m_nStopTime;        //時止め時間
    private bool m_bOnce = true;
    private int m_nTimeLimit;
    [SerializeField] private YK_MoveCursol MoveCursol;

    /**
     * @brief スタート時に呼ばれる関数
     *        初期化処理を行う
     */
    void Start()
    {
        m_eUIType = UIType.Clock; // UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        OutLine.enabled = false;
        // UIが動くようならUpdateにかかないといけない
        GetSetUIPos = Clock.GetComponent<RectTransform>().anchoredPosition;
        // スケール取得
        GetSetUIScale = Clock.transform.localScale;
        Second_Scale = Second.transform.localScale;
        //タイムリミットの取得
        m_nTimeLimit = m_Time.GetSetTimeLimit;
    }


    /**
     * @brief フレームごとに呼ばれる関数
     *        時計の針の回転や停止時間の管理を行う
     */
    void Update()
    {
        // カーソルが動き始めるまで
        if (!MoveCursol.GetSetMoveFlg)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<PointEffector2D>().enabled = false;    //エフェクターを無効にすることで道中吸い寄せられない
            return;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<PointEffector2D>().enabled = true;
        }

        // ゲームがプレイ中以外は更新しない
        if (GameManager.instance.GetSetGameState != GameState.GamePlay)
            return;
        //受け取ったfloat型の値を代入する
        Clock_Inner.fillAmount = 1.0f - m_Time.GetSetNowTime / (float)m_nTimeLimit;

        // 時計の針の回転
        Second.transform.eulerAngles = new Vector3(0, 0, (m_Time.GetSetNowTime / (float)m_nTimeLimit) * 360.0f);

        // 時止め時間かどうかの判定
        if (m_bStopTime && m_bOnce)
        {
            m_bOnce = false;
            //2秒前に呼び出すといい感じに音が止む
            Invoke(nameof(StopTimeSE), m_nStopTime - 2.0f);
            // StopTimeReleaseを5秒後に呼び出す
            Invoke(nameof(StopTimeRelease), m_nStopTime);
        }
    }

    /**
     * @brief 時止め時のSE再生
     */
    public void StopTimeSE()
    {
        // SE再生
        IS_AudioManager.instance.PlaySE(SEType.SE_StopTime_Return);
    }

    /**
     * @brief 時止め時間の解除
     */
    public void StopTimeRelease()
    {
        // SE停止
        IS_AudioManager.instance.StopSE(SEType.SE_StopTime);
        m_bStopTime = false;
        m_Time.GetSetTimeFlg = true;
        m_bOnce = true;
        m_nTimeCount--;
        //UIFadeIN();
        // BGM再生
        IS_AudioManager.instance.GetBGM(BGMType.BGM_Game).UnPause();
        //Debug.Log("元戻る");
    }

    /**
     * @brief UIのフェードイン処理
     *        時計の表示をフェードインさせる
     */
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 0秒で後X,Y方向を元の大きさに変更
        this.gameObject.transform.DOScale(GetSetUIScale, 0f);
        Second.transform.DOScale(Second_Scale, 0f);
        // 0秒でテクスチャをフェードイン
        Clock.DOFade(1f, 0f);
        Clock_Inner.DOFade(1f, 0f);
        OutLine.DOFade(1f, 0f);
        Second_Image.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeIN終了");
        });
    }

    /**
     * @brief UIのフェードアウト処理
     *        時計の表示をフェードアウトさせる
     */
    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // m_fDelTime秒でm_MinScaleに変更
        this.gameObject.transform.DOScale(m_MinScale, m_fDelTime);
        Second.transform.DOScale(Second_Scale - m_MinScale, m_fDelTime);
        // m_fDelTime秒でテクスチャをフェードイン
        Clock.DOFade(0f, m_fDelTime);
        Clock_Inner.DOFade(0f, m_fDelTime);
        OutLine.DOFade(0f, m_fDelTime);
        Second_Image.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeOUT終了");
        });
    }

    /**
     * @brief 使用回数を回復する
     * @param[in] Heal 回復する回数
     */
    public void HealTimeCount(int Heal)
    {
        if (m_nTimeCount > 3)
            m_nTimeCount += Heal;
    }

    /**
     * @fn
     * 時止めフラグのgetter・setter
     * @return m_bStopTime(bool)
     *         時止めフラグの値を返す
     * @brief 時止めフラグの値を返す・セットする
     */
    public bool GetSetStopTime
    {
        get { return m_bStopTime; }
        set { m_bStopTime = value; }
    }

    /**
     * @fn
     * 時止め回数のgetter・setter
     * @return m_nTimeCount(int)
     *         時止め回数の値を返す
     * @brief 時止め回数の値を返す・セットする
     */
    public int GetSetTimeCount
    {
        get { return m_nTimeCount; }
        set { m_nTimeCount = value; }
    }

    /**
     * @brief トリガーオブジェクトとの衝突判定時に呼ばれる関数
     * @param[in] collision 衝突したオブジェクトのコライダー
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Cursol")
        {
            OutLine.enabled = true;
        }
    }

    /**
     * @brief トリガーオブジェクトとの衝突解除時に呼ばれる関数
     * @param[in] collision 衝突解除したオブジェクトのコライダー
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        OutLine.enabled = false;
    }
}
