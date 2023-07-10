/**
 * @file YK_Exit.cs
 * @brief ExitUIの処理を管理するクラス
 *        ゲーム終了時に呼び出され、UIのフェードアウトや終了処理を行う
 *        エフェクターを無効にすることで道中での吸い寄せを防ぐ
 *        ゲームプレイ中のみ更新
 *        ゲーム終了時にアプリケーションを終了
 *        表示非表示のフラグを管理
 * @author 吉田叶聖
 * @date 2023/05/02
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class YK_Exit : YK_UI
{
    [SerializeField] private Image Exit;
    private bool m_bVisibleRetry = false;
    [SerializeField] private YK_MoveCursol MoveCursol;

    /**
     * @brief Start関数
     *        UIのタイプを設定し、UIの位置とスケールを初期化
     */
    void Start()
    {
        m_eUIType = UIType.Exit; // UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        GetSetUIPos = Exit.GetComponent<RectTransform>().anchoredPosition; // UIの位置を取得
        GetSetUIScale = Exit.transform.localScale; // UIのスケールを取得
    }

    /**
     * @brief Update関数
     *        カーソルが動き始めるまでやゲームがプレイ中でない場合は更新
     *        エフェクターを無効にして道中での吸い寄せを防ぐ
     */
    private void Update()
    {
        if (!MoveCursol.GetSetMoveFlg || GameManager.instance.GetSetGameState != GameState.GameStart)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<PointEffector2D>().enabled = false; // エフェクターを無効にすることで道中での吸い寄せを防止
            return;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<PointEffector2D>().enabled = true;
        }
    }

    /**
     * @brief UIFadeOUT関数はUIのフェードアウト処理
     *        指定した時間でUIのスケールとアルファ値を変化させ、フェードアウト
     *        フェードアウトが完了したらExitPlay関数を呼び出し、ゲーム終了処理を実行
     */
    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        Exit.transform.DOScale(m_MinScale, m_fDelTime); // 指定した時間でUIのスケールを変更
        Exit.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone; // フェード処理終了時にフェード状態をリセット
            ExitPlay(); // ゲーム終了処理を実行
            Debug.Log("FadeOUT終了");
        });
    }

    /**
     * @fn
     * 表示非表示のgetter・setter
     * @return m_bVisibleRetry(bool)
     * @brief UIの表示・非表示のフラグを取得または設定
     */
    public bool GetSetVisibleFlg
    {
        get { return m_bVisibleRetry; }
        set { m_bVisibleRetry = value; }
    }

    /**
     * @brief アプリケーションを終了
     */
    public void ExitPlay()
    {
        Application.Quit(); // アプリケーションを終了する
    }
}
