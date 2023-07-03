/**
 * @file YK_Retry.cs
 * @brief RetryUIの処理を管理するクラスです。
 *        リトライUIの表示と非表示
 *        リトライUIのフェードインとフェードアウトを制御
 *        DOTweenライブラリを使用します。 
 * @author 吉田叶聖
 * @date 2023/05/02
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YK_Retry : YK_UI
{
    [SerializeField] Fade fade;
    [SerializeField] private Image RetryUI;
    private bool m_bVisibleRetry = false;
    [SerializeField] private YK_JsonSave Data;

    /**
     * @brief Start関数
     *        UIのタイプを設定し、UIの位置とスケールを初期化
     */
    void Start()
    {
        m_eUIType = UIType.Retry; // UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        GetSetUIPos = RetryUI.GetComponent<RectTransform>().anchoredPosition; // UIの位置を取得
        GetSetUIScale = RetryUI.transform.localScale; // UIのスケールを取得
    }

    /**
     * @brief UIFadeIN関数はUIのフェードイン処理
     *        UIのスケールとアルファ値を変化させてフェードイン
     *        フェードインが完了したらフェード状態を設定
     */
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        RetryUI.transform.DOScale(GetSetUIScale, 0f); // UIのスケールを変更して元の大きさに戻す
        RetryUI.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone; // フェード処理終了時にフェード状態をリセット
            Debug.Log("FadeIN終了");
        });
    }

    /**
     * @brief UIFadeOUT関数はUIのフェードアウト処理
     *        UIのスケールとアルファ値を変化させてフェードアウト
     *        フェードアウトが完了したらフェード状態を設定
     *        フェードアウトが終了した後にリトライを行う
     */
    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        RetryUI.transform.DOScale(m_MinScale, m_fDelTime); // UIのスケールを変更して縮小する
        RetryUI.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone; // フェード処理終了時にフェード状態をリセット
            RetryPlay(); // リトライを実行
            Debug.Log("FadeOUT終了");
        });
    }

    /**
     * @fn
     * 表示非表示のgetter・setter
     * @return m_bVisibleRetry(bool)
     * @brief リトライUIの可視性を取得または設定
     */
    public bool GetSetVisibleFlg
    {
        get { return m_bVisibleRetry; }
        set { m_bVisibleRetry = value; }
    }

    /**
     * @brief RetryPlay関数はリトライ時の処理を行う
     *        フェードイン中に使用されるトランジションを掛けて指定のシーンをリトライ
     *        リトライ時にBGMを停止
     */
    public void RetryPlay()
    {
        fade.FadeIn(1f, () =>
        {
            Data.Load();
            IS_AudioManager.instance.StopBGM(BGMType.BGM_GAMEOVER); // BGMを停止する
            SceneManager.LoadScene("GameScene"); // 指定のシーンを読み込む
            // デバック用
           // SceneManager.LoadScene("KanaseScene"); // 指定のシーンを読み込む
        });
    }
}
