/**
 * @file   YK_GameOver.cs
 * @brief  ゲームオーバークラス
           ゲームオーバーの状態を管理し、ゲームオーバー時の処理を制御
 * @author YoshidaKanase
 * @date   2023/04/26
 * @Update 2023/05/05 ゲームオーバーのBGM実装
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum GameOverState
{
    BreakHPBar,   // HPバー破壊
    NoHP,         // HPがゼロ
    TimeLimit,    // 制限時間の経過

    MaxGameOverState
}

public class YK_GameOver : MonoBehaviour
{
    public static YK_GameOver instance;

    [SerializeField] private GameObject GameOver;  // ゲームオーバーUIのオブジェクト
    [SerializeField] private YK_Clock clock;       // ゲーム内の時計オブジェクト
    [SerializeField] private ON_TextEntry TextEntry;  // テキストエントリオブジェクト
    private bool m_bGameOverFlg;  // ゲームオーバーフラグ
    [SerializeField] private GameOverState m_GameOverState;  // ゲームオーバーの状態
    [SerializeField] private CanvasGroup TextCanvas;  // テキストを表示するキャンバスグループ
    [SerializeField] private Text HPBarBroken;  // HPバー破壊時のテキスト
    [SerializeField] private Text NoHP;  // HPがゼロの時のテキスト
    [SerializeField] private Text TimeLimit;  // 制限時間経過時のテキスト
    private Vector2 FirstPos;  // テキストの初期位置
    [SerializeField] private float m_fFadeTime;  // テキストのフェード時間
    private float m_rate = 0.0f;  // フェード率
    private float m_fTime;  // 経過時間

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        GameOver.SetActive(false);  // ゲームオーバーUIを非表示にする
        m_bGameOverFlg = false;  // ゲームオーバーフラグを初期化する
        HPBarBroken.DOFade(0f, 0f);  // HPバー破壊時のテキストを非表示にする
        NoHP.DOFade(0f, 0f);  // HPがゼロの時のテキストを非表示にする
        TimeLimit.DOFade(0f, 0f);  // 制限時間経過時のテキストを非表示にする
        FirstPos = TextCanvas.GetComponent<RectTransform>().anchoredPosition;  // テキストの初期位置を保存する
    }

    void Update()
    {
        if (GameManager.instance.GetSetGameState == GameState.GameOver)
        {
            m_fTime += Time.deltaTime;
            m_rate = Mathf.Lerp(0.0f, 0.5f, m_fTime / 4);  // フェード率を時間に基づいて変化させる
            TextEntry.SetRate(m_rate);  // テキストエントリのフェード率を更新する
        }
        if (GameManager.instance.GetSetGameState == GameState.GameOver && !m_bGameOverFlg)
        {
            TextCanvas.gameObject.SetActive(true);  // テキストを表示するキャンバスをアクティブにする
            FadeIN();  // テキストのフェードイン処理を実行する
            if (!m_bGameOverFlg)
            {
                IS_AudioManager.instance.AllStopSE();  // すべてのSEを停止する
                IS_AudioManager.instance.AllStopBGM();  // すべてのBGMを停止する
                IS_AudioManager.instance.PlayBGM(BGMType.BGM_GAMEOVER);  // ゲームオーバーのBGMを再生する
            }
            m_bGameOverFlg = true;  // ゲームオーバーフラグを設定する
            GameOver.SetActive(true);  // ゲームオーバーUIを表示する
        }
    }

    //Ruleを表示
    public void FadeIN()
    {
        switch (m_GameOverState)
        {
            case GameOverState.BreakHPBar:
                HPBarBroken.DOFade(1f, m_fFadeTime).OnComplete(() =>
                {
                    HPBarBroken.DOFade(0f, m_fFadeTime);  // テキストのフェードアウト処理を実行する
                });
                break;
            case GameOverState.NoHP:
                NoHP.DOFade(1f, m_fFadeTime).OnComplete(() =>
                {
                    NoHP.DOFade(0f, m_fFadeTime);  // テキストのフェードアウト処理を実行する
                });
                break;
            case GameOverState.TimeLimit:
                TimeLimit.DOFade(1f, m_fFadeTime).OnComplete(() =>
                {
                    TimeLimit.DOFade(0f, m_fFadeTime);  // テキストのフェードアウト処理を実行する
                });
                break;
        }
    }

    /**
     * @fn
     * ゲームオーバーの状態のgetter・setter
     * @return m_GameState
     * @brief ゲームオーバーの状態を返す・セットする
     */
    public GameOverState GetSetGameOverState
    {
        get { return m_GameOverState; }
        set { m_GameOverState = value; }
    }
}

